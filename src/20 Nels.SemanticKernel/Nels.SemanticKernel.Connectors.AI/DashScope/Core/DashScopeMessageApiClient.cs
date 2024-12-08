// Copyright (c) Microsoft. All rights reserved.

using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Http;
using Nels.SemanticKernel.DashScope.Core.Models;
using Nels.SemanticKernel.DashScope.Models;
using Nels.SemanticKernel.InternalUtilities.Http;
using Nels.SemanticKernel.InternalUtilities.Text;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Nels.SemanticKernel.DashScope.Core;

/// <summary>
/// This class is responsible for making HTTP requests to the DashScope Inference API - Chat Completion Message API
/// <see href="https://help.aliyun.com/zh/dashscope/developer-reference/use-qwen?spm=a2c4g.11186623.0.0.398146c11G69cy" />
/// </summary>
internal sealed class DashScopeMessageApiClient
{
    private readonly DashScopeClient _clientCore;

    private static readonly string s_namespace = typeof(DashScopeMessageApiClient).Namespace!;

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

    internal DashScopeMessageApiClient(
        string modelId,
        HttpClient httpClient,
        Uri endpoint = null,
        string apiKey = null,
        ILogger logger = null)
    {
        _clientCore = new DashScopeClient(
            modelId,
            httpClient,
            endpoint,
            apiKey,
            logger);
    }

    internal async IAsyncEnumerable<StreamingChatMessageContent> StreamCompleteChatMessageAsync(
      ChatHistory chatHistory,
      PromptExecutionSettings executionSettings,
      [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        string modelId = executionSettings?.ModelId ?? _clientCore.ModelId;
        var endpoint = GetChatGenerationEndpoint();
        var request = CreateChatRequest(chatHistory, executionSettings);
        request.Parameters.IncrementalOutput = true;
        request.Model = modelId;

        using var httpRequestMessage = _clientCore.CreatePost(request, endpoint, _clientCore.ApiKey);
        httpRequestMessage.Headers.Add("X-DashScope-SSE", "enable");

        using var response = await _clientCore.SendRequestAndGetResponseImmediatelyAfterHeadersReadAsync(httpRequestMessage, cancellationToken)
            .ConfigureAwait(false);

        using var responseStream = await response.Content.ReadAsStreamAndTranslateExceptionAsync()
            .ConfigureAwait(false);

        await foreach (var streamingChatContent in ProcessChatResponseStreamAsync(responseStream, modelId, cancellationToken).ConfigureAwait(false))
        {
            yield return streamingChatContent;
        }
    }

    internal async Task<IReadOnlyList<ChatMessageContent>> CompleteChatMessageAsync(
        ChatHistory chatHistory,
        PromptExecutionSettings executionSettings,
        CancellationToken cancellationToken)
    {
        string modelId = executionSettings?.ModelId ?? _clientCore.ModelId;
        var endpoint = GetChatGenerationEndpoint();
        var request = CreateChatRequest(chatHistory, executionSettings);
        using var httpRequestMessage = _clientCore.CreatePost(request, endpoint, _clientCore.ApiKey);

        string body = await _clientCore.SendRequestAndGetStringBodyAsync(httpRequestMessage, cancellationToken)
            .ConfigureAwait(false);

        var response = DashScopeClient.DeserializeResponse<ChatCompletionResponse>(body);
        var chatContents = GetChatMessageContentsFromResponse(response, modelId);

        LogChatCompletionUsage(executionSettings, response);

        return chatContents;
    }

    private void LogChatCompletionUsage(PromptExecutionSettings executionSettings, ChatCompletionResponse chatCompletionResponse)
    {
        if (_clientCore.Logger.IsEnabled(LogLevel.Debug))
        {
            _clientCore.Logger.Log(
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

    public static List<ChatMessageContent> GetChatMessageContentsFromResponse(ChatCompletionResponse response, string modelId)
    {
        var chatMessageContents = new List<ChatMessageContent>();
        foreach (var choice in response.Output.Choices!)
        {
            var metadata = new DashScopeChatCompletionMetadata
            {
                Created = response.Output.Created,
                FinishReason = choice.FinishReason,
                UsageCompletionTokens = response.Usage?.OutputTokens,
                UsagePromptTokens = response.Usage?.InputTokens,
                UsageTotalTokens = response.Usage?.TotalTokens,
            };

            chatMessageContents.Add(new ChatMessageContent(
                role: new AuthorRole(choice.Message?.Role ?? AuthorRole.Assistant.ToString()),
                content: choice.Message?.Content,
                modelId: modelId,
                innerContent: response,
                encoding: Encoding.UTF8,
                metadata: metadata));
        }

        return chatMessageContents;
    }

    public static StreamingChatMessageContent GetStreamingChatMessageContentFromStreamResponse(ChatCompletionResponse response, string modelId)
    {
        var choice = response?.Output?.Choices?.FirstOrDefault();
        if (response != null && choice != null && choice.Message != null)
        {
            var metadata = new DashScopeChatCompletionMetadata
            {
                Created = response.Output.Created,
                FinishReason = choice.FinishReason,
                UsageCompletionTokens = response.Usage?.OutputTokens,
                UsagePromptTokens = response.Usage?.InputTokens,
                UsageTotalTokens = response.Usage?.TotalTokens,
            };

            var streamChat = new StreamingChatMessageContent(
                new AuthorRole(choice.Message.Role),
                choice.Message.Content,
                response.Output,
                0,
                modelId,
                Encoding.UTF8,
                metadata);

            return streamChat;
        }

        throw new KernelException("Unexpected response from model")
        {
            Data = { { "ResponseData", response } },
        };
    }

    private async IAsyncEnumerable<StreamingChatMessageContent> ProcessChatResponseStreamAsync(Stream stream, string modelId, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        await foreach (var content in ParseChatResponseStreamAsync(stream, cancellationToken).ConfigureAwait(false))
        {
            yield return GetStreamingChatMessageContentFromStreamResponse(content, modelId);
        }
    }

    public ChatCompletionRequest CreateChatRequest(
        ChatHistory chatHistory,
        PromptExecutionSettings promptExecutionSettings)
    {
        var executionSettings = DashScopePromptExecutionSettings.FromExecutionSettings(promptExecutionSettings);
        executionSettings.ModelId ??= _clientCore.ModelId;

        DashScopeClient.ValidateMaxTokens(executionSettings.MaxTokens);
        var request = ChatCompletionRequest.FromChatHistoryAndExecutionSettings(chatHistory, executionSettings, null);
        request.Parameters.IncrementalOutput = false;
        return request;
    }

    public static IAsyncEnumerable<ChatCompletionResponse> ParseChatResponseStreamAsync(Stream responseStream, CancellationToken cancellationToken)
        => SseJsonParser.ParseAsync<ChatCompletionResponse>(responseStream, cancellationToken);

    private Uri GetChatGenerationEndpoint()
        => new Uri($"{_clientCore.Endpoint}{_clientCore.Separator}api/v1/services/aigc/text-generation/generation");
}
