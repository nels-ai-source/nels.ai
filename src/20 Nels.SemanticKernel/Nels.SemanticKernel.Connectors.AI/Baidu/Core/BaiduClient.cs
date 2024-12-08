using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Http;
using Nels.SemanticKernel.Baidu.Core.Models;
using Nels.SemanticKernel.InternalUtilities.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Nels.SemanticKernel.Baidu.Core;

internal sealed class BaiduClient
{
    private readonly HttpClient _httpClient;

    internal string ModelId { get; }
    internal string ApiKey { get; }
    internal string ClientId { get; }
    internal Uri Endpoint { get; }
    internal string Separator { get; }
    internal ILogger Logger { get; }



    internal BaiduClient(
        string modelId,
        HttpClient httpClient,
        Uri endpoint = null,
        string apiKey = null,
        string clientId = null,
        ILogger logger = null)
    {
        Verify.NotNullOrWhiteSpace(modelId);
        Verify.NotNull(httpClient);

        endpoint ??= new Uri("https://aip.baidubce.com");
        Separator = endpoint.AbsolutePath.EndsWith("/", StringComparison.InvariantCulture) ? string.Empty : "/";
        Endpoint = endpoint;
        ModelId = modelId;
        ApiKey = apiKey;
        ClientId = clientId;
        _httpClient = httpClient;
        Logger = logger ?? NullLogger.Instance;
    }

    #region ClientCore
    internal static void ValidateMaxTokens(int? maxTokens)
    {
        if (maxTokens < 1)
        {
            throw new ArgumentException($"MaxTokens {maxTokens} is not valid, the value must be greater than zero");
        }
    }

    internal static void ValidateMaxNewTokens(int? maxNewTokens)
    {
        if (maxNewTokens < 0)
        {
            throw new ArgumentException($"MaxNewTokens {maxNewTokens} is not valid, the value must be greater than or equal to zero");
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


    #region Embeddings
    public async Task<IList<ReadOnlyMemory<float>>> GenerateEmbeddingsAsync(
IList<string> data,
Kernel kernel,
CancellationToken cancellationToken)
    {

        string accessToken = await BaiduAccessTokenClient.GetAccessToken(this);

        var endpoint = GetEmbeddingGenerationEndpoint(ModelId, accessToken);

        if (data.Count > 1)
        {
            throw new NotSupportedException("Currently this interface does not support multiple embeddings results per data item, use only one data item");
        }

        var request = new TextEmbeddingRequest
        {
            input = data,
        };

        //request.ParseUrl(endpoint.ToString());

        using var httpRequestMessage = CreatePost(request, endpoint, ApiKey);

        string body = await SendRequestAndGetStringBodyAsync(httpRequestMessage, cancellationToken)
            .ConfigureAwait(false);
        //body是 json 判断是否存在某个属性
        if (body.Contains("error_code"))
        {
            throw new KernelException("百度大模型" + ModelId + "生成向量失败 ： " + body);
        }
        /**
         * 
         * 百度向量各大模型返回的向量长度不一样
         * 参考文档地址：https://cloud.baidu.com/doc/WENXINWORKSHOP/s/alj562vvu
         * Embedding-V1：384个float64
         * bge-large-zh：1024个float64
         * tao-8k：1024个float
         * 注意事项如下：
         * 1.milvus限制了长度是1536，所以这里如果数量不足，补齐长度
         * 2.百度返回的向量精度要double类型，但是这里限制了float，这里转换了精度！
         * **/
        // 创建一个float数组 长度1536
        float[] floatArray = new float[1536];

        List<double> embeddings = new List<double>();
        TextEmbeddingResponse textEmbeddingResponse = DeserializeResponse<TextEmbeddingResponse>(body);
        if (textEmbeddingResponse != null && textEmbeddingResponse.data != null)
        {
            //因为是分片生成向量，data.count = 1 这里只需要读取第一个即可
            embeddings = textEmbeddingResponse.data.FirstOrDefault().embedding;
        }

        for (int i = 0; i < 1536; i++)
        {
            if (i >= embeddings.Count)
            {
                floatArray[i] = 0.00f;
            }
            else
            {
                floatArray[i] = (float)embeddings[i];
            }
        }
        // 使用float数组创建ReadOnlyMemory<float>实例
        ReadOnlyMemory<float> readOnlyMemory = new ReadOnlyMemory<float>(floatArray);

        List<ReadOnlyMemory<float>> result = new List<ReadOnlyMemory<float>>();

        result.Add(readOnlyMemory);

        return result;
    }
    private Uri GetEmbeddingGenerationEndpoint(string modelId, string access_token) => new Uri($"{Endpoint}rpc/2.0/ai_custom/v1/wenxinworkshop/embeddings/{modelId}?access_token={access_token}");

    #endregion
}
