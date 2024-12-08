// Copyright (c) Microsoft. All rights reserved.

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Http;
using Nels.SemanticKernel.InternalUtilities.Http;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Nels.SemanticKernel.DeepSeek.Core;

internal sealed class DeepSeekClient
{
    private readonly HttpClient _httpClient;

    internal string ModelId { get; }
    internal string ApiKey { get; }
    internal Uri Endpoint { get; }
    internal string Separator { get; }
    internal ILogger Logger { get; }

    internal DeepSeekClient(
        string modelId,
        HttpClient httpClient,
        Uri endpoint = null,
        string apiKey = null,
        ILogger logger = null)
    {
        Verify.NotNullOrWhiteSpace(modelId);
        Verify.NotNull(httpClient);

        endpoint ??= new Uri("https://qianxun.rcrai.com");
        Separator = endpoint.AbsolutePath.EndsWith("/", StringComparison.InvariantCulture) ? string.Empty : "/";
        Endpoint = endpoint;
        ModelId = modelId;
        ApiKey = apiKey;
        _httpClient = httpClient;
        Logger = logger ?? NullLogger.Instance;
    }

    #region ClientCore
    internal static void ValidateMaxTokens(int? maxTokens)
    {
        if (maxTokens.HasValue && maxTokens < 1)
        {
            throw new ArgumentException($"MaxTokens {maxTokens} is not valid, the value must be greater than zero");
        }
    }

    internal async Task<string> SendRequestAndGetStringBodyAsync(
        HttpRequestMessage httpRequestMessage,
        CancellationToken cancellationToken)
    {
        using var response = await _httpClient.SendWithSuccessCheckAsync(httpRequestMessage, cancellationToken)
            .ConfigureAwait(false);

        var body = await response.Content.ReadAsStringWithExceptionMappingAsync()
            .ConfigureAwait(false);

        return body;
    }

    internal async Task<HttpResponseMessage> SendRequestAndGetResponseImmediatelyAfterHeadersReadAsync(
        HttpRequestMessage httpRequestMessage,
        CancellationToken cancellationToken)
    {
        var response = await _httpClient.SendWithSuccessCheckAsync(httpRequestMessage, HttpCompletionOption.ResponseHeadersRead, cancellationToken)
            .ConfigureAwait(false);
        return response;
    }

    internal static T DeserializeResponse<T>(string body)
    {
        try
        {
            var deserializedResponse = JsonSerializer.Deserialize<T>(body);
            if (deserializedResponse is null)
            {
                throw new JsonException("Response is null");
            }

            return deserializedResponse;
        }
        catch (JsonException exc)
        {
            throw new KernelException("Unexpected response from model", exc)
            {
                Data = { { "ResponseData", body } },
            };
        }
    }

    internal void SetRequestHeaders(HttpRequestMessage request)
    {
        request.Headers.Add("User-Agent", HttpHeaderConstant.Values.UserAgent);
        request.Headers.Add(HttpHeaderConstant.Names.SemanticKernelVersion, HttpHeaderConstant.Values.GetAssemblyVersion(GetType()));
        if (!string.IsNullOrEmpty(ApiKey))
        {
            request.Headers.Add("Authorization", $"Bearer {ApiKey}");
        }
    }

    internal HttpRequestMessage CreatePost(object requestData, Uri endpoint, string apiKey)
    {
        var httpRequestMessage = HttpRequest.CreatePostRequest(endpoint, requestData);
        SetRequestHeaders(httpRequestMessage);

        return httpRequestMessage;
    }

    #endregion
}
