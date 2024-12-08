using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Http;
using Nels.SemanticKernel.Baidu.Core.Models;
using Nels.SemanticKernel.Baidu.Models;
using Nels.SemanticKernel.InternalUtilities.Http;
using Nels.SemanticKernel.InternalUtilities.Text;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.IO;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Nels.SemanticKernel.Baidu.Core;

internal sealed class BaiduMessageApiClient
{
    private readonly BaiduClient _clientCore;

    private static readonly string s_namespace = typeof(BaiduMessageApiClient).Namespace!;

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

    internal BaiduMessageApiClient(
        string modelId,
        HttpClient httpClient,
        Uri endpoint = null,
        string apiKey = null,
        string clientID = null,
        ILogger logger = null)
    {
        _clientCore = new BaiduClient(
            modelId,
            httpClient,
            endpoint,
            apiKey,
            clientID,
            logger);
    }

    internal async IAsyncEnumerable<StreamingChatMessageContent> StreamCompleteChatMessageAsync(
      ChatHistory chatHistory,
      PromptExecutionSettings executionSettings,
      [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        string modelId = executionSettings?.ModelId ?? _clientCore.ModelId;

        string accessToken = await GetAccessToken();

        var endpoint = GetBaiduChatGenerationEndpoint(accessToken);


        var request = CreateChatRequest(chatHistory, executionSettings);

        request.Model = modelId;
        request.Stream = true;

        using var httpRequestMessage = _clientCore.CreatePost(request, endpoint, _clientCore.ApiKey);

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

        var response = BaiduClient.DeserializeResponse<ChatCompletionResponse>(body);
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
            "HuggingFace chat completion usage - ModelId: {ModelId}, Prompt tokens: {PromptTokens}, Completion tokens: {CompletionTokens}, Total tokens: {TotalTokens}",
            chatCompletionResponse.Model,
            chatCompletionResponse.Usage!.PromptTokens,
            chatCompletionResponse.Usage!.CompletionTokens,
            chatCompletionResponse.Usage!.TotalTokens);
        }

        s_promptTokensCounter.Add(chatCompletionResponse.Usage!.PromptTokens);
        s_completionTokensCounter.Add(chatCompletionResponse.Usage!.CompletionTokens);
        s_totalTokensCounter.Add(chatCompletionResponse.Usage!.TotalTokens);
    }

    private static List<ChatMessageContent> GetChatMessageContentsFromResponse(ChatCompletionResponse response, string modelId)
    {
        var chatMessageContents = new List<ChatMessageContent>();
        foreach (var choice in response.Choices!)
        {
            var metadata = new BaiduChatCompletionMetadata
            {
                Id = response.Id,
                Model = response.Model,
                @Object = response.Object,
                SystemFingerPrint = response.SystemFingerprint,
                Created = response.Created,
                FinishReason = choice.FinishReason,
                LogProbs = choice.LogProbs,
                UsageCompletionTokens = response.Usage?.CompletionTokens,
                UsagePromptTokens = response.Usage?.PromptTokens,
                UsageTotalTokens = response.Usage?.TotalTokens,
            };

            chatMessageContents.Add(new ChatMessageContent(
                role: new AuthorRole(choice.Message?.Role ?? AuthorRole.Assistant.ToString()),
                content: choice.Message?.Content,
                modelId: response.Model,
                innerContent: response,
                encoding: Encoding.UTF8,
                metadata: metadata));
        }

        return chatMessageContents;
    }

    private static StreamingChatMessageContent GetStreamingChatMessageContentFromStreamResponse(ChatCompletionStreamResponse response, string modelId)
    {
        var metadata = new BaiduChatCompletionMetadata
        {
            Id = response.Id,
            Model = modelId,
            @Object = response.Object,
            SystemFingerPrint = "",
            Created = response.Created,
            FinishReason = response.finish_reason,
            LogProbs = "",
        };

        var streamChat = new StreamingChatMessageContent(
               new AuthorRole("user"),
               response.result,
               response,
               1,
               modelId,
               Encoding.UTF8,
               metadata);
        return streamChat;

    }

    private async IAsyncEnumerable<StreamingChatMessageContent> ProcessChatResponseStreamAsync(Stream stream, string modelId, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        await foreach (var content in ParseChatResponseStreamAsync(stream, cancellationToken).ConfigureAwait(false))
        {
            yield return GetStreamingChatMessageContentFromStreamResponse(content, modelId);
        }
    }

    private ChatCompletionRequest CreateChatRequest(
        ChatHistory chatHistory,
        PromptExecutionSettings promptExecutionSettings)
    {
        var executionSettings = BaiduPromptExecutionSettings.FromExecutionSettings(promptExecutionSettings);
        executionSettings.ModelId ??= _clientCore.ModelId;

        BaiduClient.ValidateMaxTokens(executionSettings.MaxTokens);
        var request = ChatCompletionRequest.FromChatHistoryAndExecutionSettings(chatHistory, executionSettings);
        return request;
    }

    private IAsyncEnumerable<ChatCompletionStreamResponse> ParseChatResponseStreamAsync(Stream responseStream, CancellationToken cancellationToken)
        => SseJsonParser.ParseAsync<ChatCompletionStreamResponse>(responseStream, cancellationToken);

    private Uri GetChatGenerationEndpoint()
        => new Uri($"{_clientCore.Endpoint}{_clientCore.Separator}rpc/2.0/ai_custom/v1/wenxinworkshop/chat/{_clientCore.ModelId}");


    private Uri GetBaiduChatGenerationEndpoint(string access_token)
       => new Uri($"{_clientCore.Endpoint}{_clientCore.Separator}rpc/2.0/ai_custom/v1/wenxinworkshop/chat/{_clientCore.ModelId}?access_token={access_token}");

    public async Task<string> GetAccessToken()
    {
        return await BaiduAccessTokenClient.GetAccessToken(_clientCore);
    }

}
