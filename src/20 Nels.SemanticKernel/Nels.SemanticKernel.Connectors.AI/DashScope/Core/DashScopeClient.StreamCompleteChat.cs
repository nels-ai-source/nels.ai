// Copyright (c) Microsoft. All rights reserved.

using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Http;
using Nels.SemanticKernel.DashScope.Core.Models;
using Nels.SemanticKernel.DashScope.Models;
using Nels.SemanticKernel.InternalUtilities.Functions;
using Nels.SemanticKernel.InternalUtilities.Http;
using Nels.SemanticKernel.InternalUtilities.Models;
using Nels.SemanticKernel.InternalUtilities.Text;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
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

    internal async IAsyncEnumerable<StreamingChatMessageContent> StreamCompleteChatMessageAsync(
      ChatHistory chatHistory,
      PromptExecutionSettings promptExecutionSettings,
      Kernel? kernel = null,
      [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        string modelId = promptExecutionSettings?.ModelId ?? ModelId;
        var endpoint = GetChatGenerationEndpoint();
        var executionSettings = DashScopePromptExecutionSettings.FromExecutionSettings(promptExecutionSettings);
        var request = CreateChatRequest(chatHistory, executionSettings, kernel);
        request.Parameters.IncrementalOutput = true;
        request.Model = modelId;

        using var httpRequestMessage = CreatePost(request, endpoint, ApiKey);
        httpRequestMessage.Headers.Add("X-DashScope-SSE", "enable");

        using var response = await SendRequestAndGetResponseImmediatelyAfterHeadersReadAsync(httpRequestMessage, cancellationToken)
            .ConfigureAwait(false);

        using var responseStream = await response.Content.ReadAsStreamAndTranslateExceptionAsync()
            .ConfigureAwait(false);

        await foreach (var streamingChatContent in ProcessChatResponseStreamAsync(responseStream, modelId, cancellationToken).ConfigureAwait(false))
        {
            yield return streamingChatContent;
        }
    }


    public static StreamingChatMessageContent GetStreamingChatMessageContentFromStreamResponse(DashScopeChatCompletionResponse response, string modelId)
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

    public static IAsyncEnumerable<DashScopeChatCompletionResponse> ParseChatResponseStreamAsync(Stream responseStream, CancellationToken cancellationToken)
        => SseJsonParser.ParseAsync<DashScopeChatCompletionResponse>(responseStream, cancellationToken);

}
