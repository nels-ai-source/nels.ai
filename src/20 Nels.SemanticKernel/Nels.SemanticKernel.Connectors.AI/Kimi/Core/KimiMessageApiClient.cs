using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Linq;
using static Nels.SemanticKernel.Kimi.Core.Models.KimiChatCompletionResponse;
using Nels.SemanticKernel.Kimi.Core.Models;
using Nels.SemanticKernel.InternalUtilities.Http;
using Nels.SemanticKernel.Kimi.Models;
using Nels.SemanticKernel.InternalUtilities.Text;

namespace Nels.SemanticKernel.Kimi.Core;

internal sealed class KimiMessageApiClient
{
    private readonly KimiClient _clientCore;

    private static readonly string s_namespace = typeof(KimiMessageApiClient).Namespace!;

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


    internal KimiMessageApiClient(
            string modelId,
            HttpClient httpClient,
            Uri endpoint = null,
            string apiKey = null,
            ILogger logger = null)
    {
        _clientCore = new KimiClient(
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
        request.Model = modelId;

        using var httpRequestMessage = _clientCore.CreatePost(request, endpoint, _clientCore.ApiKey);

        using var response = await _clientCore.SendRequestAndGetResponseImmediatelyAfterHeadersReadAsync(httpRequestMessage, cancellationToken)
            .ConfigureAwait(false);

        using var responseStream = await response.Content.ReadAsStreamAndTranslateExceptionAsync()
            .ConfigureAwait(false);

        await foreach (var streamingChatContent in this.ProcessChatResponseStreamAsync(responseStream, modelId, cancellationToken).ConfigureAwait(false))
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

        var response = KimiClient.DeserializeResponse<KimiChatCompletionResponse>(body);
        var chatContents = GetChatMessageContentsFromResponse(response, modelId);

        LogChatCompletionUsage(executionSettings, response);

        return chatContents;
    }


    private static List<ChatMessageContent> GetChatMessageContentsFromResponse(KimiChatCompletionResponse response, string modelId)
    {
        var chatMessageContents = new List<ChatMessageContent>();
        foreach (var choice in response.Choices!)
        {
            var metadata = new KimiChatCompletionMetadata
            {
                Created = response.Created,
                FinishReason = response.Object,
                UsageCompletionTokens = response.Usage?.CompletionTokens,
                UsagePromptTokens = response.Usage?.PromptTokens,
                UsageTotalTokens = response.Usage?.TotalTokens,
            };

            chatMessageContents.Add(new ChatMessageContent(
                role: new AuthorRole(choice.Delta?.Role ?? AuthorRole.Assistant.ToString()),
                content: choice.Delta?.Content,
                modelId: modelId,
                innerContent: response,
                encoding: Encoding.UTF8,
                metadata: metadata));
        }

        return chatMessageContents;
    }


    private void LogChatCompletionUsage(PromptExecutionSettings executionSettings, KimiChatCompletionResponse chatCompletionResponse)
    {
        if (_clientCore.Logger.IsEnabled(LogLevel.Debug))
        {
            _clientCore.Logger.Log(
            LogLevel.Debug,
            "Qwen chat completion usage -  Prompt tokens: {PromptTokens}, Completion tokens: {CompletionTokens}, Total tokens: {TotalTokens}",
            chatCompletionResponse.Usage!.CompletionTokens,
            chatCompletionResponse.Usage!.PromptTokens,
            chatCompletionResponse.Usage!.TotalTokens);
        }

        s_promptTokensCounter.Add(chatCompletionResponse.Usage!.PromptTokens);
        s_completionTokensCounter.Add(chatCompletionResponse.Usage!.CompletionTokens);
        s_totalTokensCounter.Add(chatCompletionResponse.Usage!.TotalTokens);
    }


    private static StreamingChatMessageContent GetStreamingChatMessageContentFromStreamResponse(KimiChatCompletionResponse response, string modelId)
    {
        ChatChoices choice = response?.Choices.FirstOrDefault();
        if (response != null && choice != null && choice.Delta != null)
        {
            var metadata = new KimiChatCompletionMetadata
            {
                Created = response.Created,
                FinishReason = response.Object,
                UsageCompletionTokens = response.Usage?.CompletionTokens,
                UsagePromptTokens = response.Usage?.PromptTokens,
                UsageTotalTokens = response.Usage?.TotalTokens,
            };

            var streamChat = new StreamingChatMessageContent(
                new AuthorRole("user"),
                choice.Delta.Content,
                response,
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



    private KimiChatCompletionRequest CreateChatRequest(
        ChatHistory chatHistory,
        PromptExecutionSettings promptExecutionSettings)
    {
        var executionSettings = KimiPromptExecutionSettings.FromExecutionSettings(promptExecutionSettings);
        executionSettings.ModelId ??= _clientCore.ModelId;

        KimiClient.ValidateMaxTokens(executionSettings.MaxTokens);
        var request = KimiChatCompletionRequest.FromChatHistoryAndExecutionSettings(chatHistory, executionSettings);
        return request;
    }


    private IAsyncEnumerable<KimiChatCompletionResponse> ParseChatResponseStreamAsync(Stream responseStream, CancellationToken cancellationToken)
            => SseJsonParser.ParseAsync<KimiChatCompletionResponse>(responseStream, cancellationToken);


    private Uri GetChatGenerationEndpoint()
          => new Uri($"{_clientCore.Endpoint}v1/chat/completions");
}
