// Copyright (c) Microsoft. All rights reserved.

using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Nels.SemanticKernel.DashScope.Core.Models;
using Nels.SemanticKernel.DashScope.Models;
using Nels.SemanticKernel.InternalUtilities.Functions;
using Nels.SemanticKernel.InternalUtilities.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using static Nels.SemanticKernel.DashScope.Core.Models.DashScopeChatCompletionRequest;

namespace Nels.SemanticKernel.DashScope.Core;

/// <summary>
/// This class is responsible for making HTTP requests to the DashScope Inference API - Chat Completion Message API
/// <see href="https://help.aliyun.com/zh/dashscope/developer-reference/use-qwen?spm=a2c4g.11186623.0.0.398146c11G69cy" />
/// </summary>
internal partial class DashScopeClient
{
    private static readonly string s_namespace = typeof(DashScopeClient).Namespace!;
    private const string AutoValue = "auto";
    private const string NoneValue = "none";

    private const int MaxInflightAutoInvokes = 128;
    /// <summary>Tracking <see cref="AsyncLocal{Int32}"/> for <see cref="MaxInflightAutoInvokes"/>.</summary>
    private static readonly AsyncLocal<int> s_inflightAutoInvokes = new();

    /// <summary>
    /// Instance of <see cref="Meter"/> for metrics.
    /// </summary>
    private static readonly Meter s_meter = new Meter(s_namespace);

    /// <summary>
    /// Instance of <see cref="Counter{T}"/> to keep track of the number of prompt tokens used.
    /// </summary>
    private static readonly Counter<int> s_promptTokensCounter =
        s_meter.CreateCounter<int>(
            name: $"{s_namespace}.tokens.prompt",
            unit: "{token}",
            description: "Number of prompt tokens used");

    /// <summary>
    /// Instance of <see cref="Counter{T}"/> to keep track of the number of completion tokens used.
    /// </summary>
    private static readonly Counter<int> s_completionTokensCounter =
        s_meter.CreateCounter<int>(
            name: $"{s_namespace}.tokens.completion",
            unit: "{token}",
            description: "Number of completion tokens used");

    /// <summary>
    /// Instance of <see cref="Counter{T}"/> to keep track of the total number of tokens used.
    /// </summary>
    private static readonly Counter<int> s_totalTokensCounter =
        s_meter.CreateCounter<int>(
            name: $"{s_namespace}.tokens.total",
            unit: "{token}",
            description: "Number of total tokens used");


    internal async Task<IReadOnlyList<ChatMessageContent>> CompleteChatMessageAsync(
        ChatHistory chatHistory,
        PromptExecutionSettings promptExecutionSettings,
        Kernel? kernel = null,
        CancellationToken cancellationToken = default)
    {
        Verify.NotNull(chatHistory);

        if (Logger!.IsEnabled(LogLevel.Trace))
        {
            Logger.LogTrace("ChatHistory: {ChatHistory}, Settings: {Settings}",
                JsonSerializer.Serialize(chatHistory),
                JsonSerializer.Serialize(promptExecutionSettings));
        }

        ModelId = promptExecutionSettings?.ModelId ?? ModelId;
        var endpoint = GetChatGenerationEndpoint();

        var state = ValidateInputAndCreateChatCompletionState(chatHistory, kernel, promptExecutionSettings);

        for (state.Iteration = 1; ; state.Iteration++)
        {
            using var httpRequestMessage = CreatePost(state.ChatCompletionRequest, endpoint, ApiKey);

            string body = await SendRequestAndGetStringBodyAsync(httpRequestMessage, cancellationToken)
                .ConfigureAwait(false);

            var response = DashScopeClient.DeserializeResponse<DashScopeChatCompletionResponse>(body);
            var chatContents = GetChatMessageContentsFromResponse(response, ModelId);

            LogChatCompletionUsage(state.ExecutionSettings, response);

            if (!state.AutoInvoke || chatContents.Count != 1)
            {
                return chatContents;
            }
            state.LastMessage = chatContents[0];
            if (state.LastMessage.ToolCalls is null)
            {
                return chatContents;
            }

            // ToolCallBehavior is not null because we are in auto-invoke mode but we check it again to be sure it wasn't changed in the meantime
            Verify.NotNull(state.ExecutionSettings.ToolCallBehavior);

            state.AddLastMessageToChatHistoryAndRequest();
            await ProcessFunctionsAsync(state, cancellationToken).ConfigureAwait(false);
        }
    }

    private void LogChatCompletionUsage(PromptExecutionSettings executionSettings, DashScopeChatCompletionResponse chatCompletionResponse)
    {
        if (Logger.IsEnabled(LogLevel.Debug))
        {
            Logger.Log(
            LogLevel.Debug,
            "Qwen chat completion usage -  Prompt tokens: {PromptTokens}, Completion tokens: {CompletionTokens}, Total tokens: {TotalTokens}",
            chatCompletionResponse.Usage!.InputTokens,
            chatCompletionResponse.Usage!.OutputTokens,
            chatCompletionResponse.Usage!.TotalTokens);
        }

        s_promptTokensCounter.Add(chatCompletionResponse.Usage!.InputTokens);
        s_completionTokensCounter.Add(chatCompletionResponse.Usage!.OutputTokens);
        s_totalTokensCounter.Add(chatCompletionResponse.Usage!.TotalTokens);
    }

    public static List<DashScopeChatMessageContent> GetChatMessageContentsFromResponse(DashScopeChatCompletionResponse response, string modelId)
    {
        var chatMessageContents = new List<DashScopeChatMessageContent>();
        foreach (var choice in response.Output.Choices!)
        {
            var metadata = new ChatCompletionMetadata
            {
                Created = response.Output.Created,
                FinishReason = choice.FinishReason,
                UsageCompletionTokens = response.Usage?.OutputTokens,
                UsagePromptTokens = response.Usage?.InputTokens,
                UsageTotalTokens = response.Usage?.TotalTokens,
            };

            chatMessageContents.Add(new DashScopeChatMessageContent(
                role: new AuthorRole(choice.Message?.Role ?? AuthorRole.Assistant.ToString()),
                content: choice.Message?.Content,
                modelId: modelId,
                functionsToolCalls: choice.Message.ToolCalls,
                metadata: metadata));
        }

        return chatMessageContents;
    }

    private async Task ProcessFunctionsAsync(ChatCompletionState state, CancellationToken cancellationToken)
    {
        if (Logger.IsEnabled(LogLevel.Debug))
        {
            Logger.LogDebug("Tool requests: {Requests}", state.LastMessage!.ToolCalls!.Count);
        }

        if (Logger.IsEnabled(LogLevel.Trace))
        {
            Logger.LogTrace("Function call requests: {FunctionCall}",
                string.Join(", ", state.LastMessage!.ToolCalls!.Select(ftc => ftc.ToString())));
        }

        // We must send back a response for every tool call, regardless of whether we successfully executed it or not.
        // If we successfully execute it, we'll add the result. If we don't, we'll add an error.
        foreach (var toolCall in state.LastMessage!.ToolCalls!)
        {
            await ProcessSingleToolCallAsync(state, toolCall, cancellationToken).ConfigureAwait(false);
        }

        // Clear the tools. If we end up wanting to use tools, we'll reset it to the desired value.
        state.ChatCompletionRequest.Parameters.Tools = null;

        if (state.Iteration >= state.ExecutionSettings.ToolCallBehavior!.MaximumUseAttempts)
        {
            // Don't add any tools as we've reached the maximum attempts limit.
            if (Logger.IsEnabled(LogLevel.Debug))
            {
                Logger.LogDebug("Maximum use ({MaximumUse}) reached; removing the tools.",
                    state.ExecutionSettings.ToolCallBehavior!.MaximumUseAttempts);
            }
        }
        else
        {
            // Regenerate the tool list as necessary. The invocation of the function(s) could have augmented
            // what functions are available in the kernel.
            state.ExecutionSettings.ToolCallBehavior!.ConfigureDashScopeRequest(state.Kernel, state.ChatCompletionRequest);
        }

        // Disable auto invocation if we've exceeded the allowed limit.
        if (state.Iteration >= state.ExecutionSettings.ToolCallBehavior!.MaximumAutoInvokeAttempts)
        {
            state.AutoInvoke = false;
            if (Logger.IsEnabled(LogLevel.Debug))
            {
                Logger.LogDebug("Maximum auto-invoke ({MaximumAutoInvoke}) reached.",
                    state.ExecutionSettings.ToolCallBehavior!.MaximumAutoInvokeAttempts);
            }
        }
    }


    private async Task ProcessSingleToolCallAsync(ChatCompletionState state, FunctionToolCall toolCall, CancellationToken cancellationToken)
    {
        // Make sure the requested function is one we requested. If we're permitting any kernel function to be invoked,
        // then we don't need to check this, as it'll be handled when we look up the function in the kernel to be able
        // to invoke it. If we're permitting only a specific list of functions, though, then we need to explicitly check.
        if (state.ExecutionSettings.ToolCallBehavior?.AllowAnyRequestedKernelFunction is not true &&
            !IsRequestableTool(state.ChatCompletionRequest.Parameters.Tools.Select(x => x.Function), toolCall))
        {
            AddToolResponseMessage(state.ChatHistory, state.ChatCompletionRequest, toolCall, functionResponse: null,
                "Error: Function call request for a function that wasn't defined.");
            return;
        }

        // Ensure the provided function exists for calling
        if (!state.Kernel!.Plugins.TryGetFunctionAndArguments(toolCall, out KernelFunction? function, out KernelArguments? functionArgs))
        {
            AddToolResponseMessage(state.ChatHistory, state.ChatCompletionRequest, toolCall, functionResponse: null,
                "Error: Requested function could not be found.");
            return;
        }

        // Now, invoke the function, and add the resulting tool call message to the chat history.
        s_inflightAutoInvokes.Value++;
        FunctionResult? functionResult;
        try
        {
            // Note that we explicitly do not use executionSettings here; those pertain to the all-up operation and not necessarily to any
            // further calls made as part of this function invocation. In particular, we must not use function calling settings naively here,
            // as the called function could in turn telling the model about itself as a possible candidate for invocation.
            functionResult = await function.InvokeAsync(state.Kernel, functionArgs, cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }
        catch (Exception e)
        {
            AddToolResponseMessage(state.ChatHistory, state.ChatCompletionRequest, toolCall, functionResponse: null,
                $"Error: Exception while invoking function. {e.Message}");
            return;
        }
        finally
        {
            s_inflightAutoInvokes.Value--;
        }

        AddToolResponseMessage(state.ChatHistory, state.ChatCompletionRequest, toolCall,
            functionResponse: functionResult, errorMessage: null);
    }

    /// <summary>
    /// Checks if a tool call is for a function that was defined.
    /// </summary>
    private static bool IsRequestableTool(IEnumerable<FunctionDeclaration> functions, FunctionToolCall ftc)
        => functions.Any(geminiFunction =>
            string.Equals(geminiFunction.Name, ftc.FullyQualifiedName, StringComparison.OrdinalIgnoreCase));

    private void AddToolResponseMessage(ChatHistory chat, DashScopeChatCompletionRequest request, FunctionToolCall tool, FunctionResult? functionResponse, string? errorMessage)
    {
        if (errorMessage is not null && Logger.IsEnabled(LogLevel.Debug))
        {
            Logger.LogDebug("Failed to handle tool request ({ToolName}). {Error}", tool.FullyQualifiedName, errorMessage);
        }

        var message = new DashScopeChatMessageContent(AuthorRole.Tool,
            content: errorMessage ?? string.Empty,
            modelId: ModelId,
            calledToolResult: functionResponse,
            metadata: null);
        chat.Add(message);
        request.AddChatMessage(message);
    }



    public DashScopeChatCompletionRequest CreateChatRequest(
        ChatHistory chatHistory,
        DashScopePromptExecutionSettings executionSettings,
        Kernel? kernel)
    {
        executionSettings.ModelId ??= ModelId;

        ValidateMaxTokens(executionSettings.MaxTokens);
        var request = FromChatHistoryAndExecutionSettings(chatHistory, executionSettings, null);
        request.Parameters.IncrementalOutput = false;

        FunctionCallingConfiguration(request, chatHistory, executionSettings, kernel);

        return request;
    }

    private Uri GetChatGenerationEndpoint()
        => new($"{Endpoint}{Separator}api/v1/services/aigc/text-generation/generation");

    private static void FunctionCallingConfiguration(DashScopeChatCompletionRequest request, ChatHistory chatHistory, DashScopePromptExecutionSettings executionSettings, Kernel? kernel)
    {
        executionSettings.ToolCallBehavior?.ConfigureDashScopeRequest(kernel, request);

        // If neither behavior is specified, we just return default configuration with no tool and no choice
        if (executionSettings.FunctionChoiceBehavior is null && executionSettings.ToolCallBehavior is null)
        {
            return;
        }
        // If both behaviors are specified, we can't handle that.
        if (executionSettings.FunctionChoiceBehavior is not null && executionSettings.ToolCallBehavior is not null)
        {
            throw new ArgumentException($"{nameof(executionSettings.ToolCallBehavior)} and {nameof(executionSettings.FunctionChoiceBehavior)} cannot be used together.");
        }
        var configuration = executionSettings.FunctionChoiceBehavior.GetConfiguration(new(chatHistory) { Kernel = kernel });
        // Disable auto invocation if no kernel is provided.
        configuration.AutoInvoke = kernel is not null && configuration.AutoInvoke;

        if (configuration.Choice == FunctionChoice.Auto)
        {
            request.ToolChoice = AutoValue;
        }
        else if (configuration.Choice == FunctionChoice.None)
        {
            request.ToolChoice = NoneValue;
        }
        if (executionSettings.ExtensionData.TryGetValue("tool_choice", out object value))
        {
            request.ToolChoice = JsonSerializer.Serialize((new DashScopeTool { Function = new FunctionDeclaration { Name = value.ToString() } }));
        }

    }

    private ChatCompletionState ValidateInputAndCreateChatCompletionState(
       ChatHistory chatHistory,
       Kernel? kernel,
       PromptExecutionSettings? executionSettings)
    {
        ValidateChatHistory(chatHistory);

        var dashScopeExecutionSettings = DashScopePromptExecutionSettings.FromExecutionSettings(executionSettings);
        ValidateMaxTokens(dashScopeExecutionSettings.MaxTokens);

        if (Logger.IsEnabled(LogLevel.Trace))
        {
            Logger.LogTrace("ChatHistory: {ChatHistory}, Settings: {Settings}",
                JsonSerializer.Serialize(chatHistory),
                JsonSerializer.Serialize(dashScopeExecutionSettings));
        }

        return new ChatCompletionState()
        {
            AutoInvoke = CheckAutoInvokeCondition(kernel, dashScopeExecutionSettings),
            ChatHistory = chatHistory,
            ExecutionSettings = dashScopeExecutionSettings,
            ChatCompletionRequest = CreateChatRequest(chatHistory, dashScopeExecutionSettings, kernel),
            Kernel = kernel! // not null if auto-invoke is true
        };
    }

    private static void ValidateChatHistory(ChatHistory chatHistory)
    {
        Verify.NotNullOrEmpty(chatHistory);
        if (chatHistory.All(message => message.Role == AuthorRole.System))
        {
            throw new InvalidOperationException("Chat history can't contain only system messages.");
        }
    }
    private static bool CheckAutoInvokeCondition(Kernel? kernel, DashScopePromptExecutionSettings executionSettings)
    {
        bool autoInvoke = kernel is not null
                          && executionSettings.ToolCallBehavior?.MaximumAutoInvokeAttempts > 0
                          && s_inflightAutoInvokes.Value < MaxInflightAutoInvokes;
        return autoInvoke;
    }
    private sealed class ChatCompletionState
    {
        internal ChatHistory ChatHistory { get; set; } = null!;
        internal DashScopeChatCompletionRequest ChatCompletionRequest { get; set; } = null!;
        internal Kernel Kernel { get; set; } = null!;
        internal DashScopePromptExecutionSettings ExecutionSettings { get; set; } = null!;
        internal DashScopeChatMessageContent? LastMessage { get; set; }
        internal int Iteration { get; set; }
        internal bool AutoInvoke { get; set; }

        internal void AddLastMessageToChatHistoryAndRequest()
        {
            Verify.NotNull(LastMessage);
            ChatHistory.Add(LastMessage);
            ChatCompletionRequest.AddChatMessage(LastMessage);
        }
    }
}
