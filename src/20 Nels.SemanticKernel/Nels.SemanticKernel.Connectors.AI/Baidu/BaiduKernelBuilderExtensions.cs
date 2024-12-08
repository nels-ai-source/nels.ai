using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Embeddings;
using Nels.SemanticKernel.Baidu.Services;
using Nels.SemanticKernel.InternalUtilities.Http;
using System;
using System.Net.Http;

namespace Nels.SemanticKernel.Baidu;

public static class BaiduKernelBuilderExtensions
{
    /// <summary>
    /// Adds an Hugging Face chat completion service with the specified configuration.
    /// </summary>
    /// <param name="builder">The <see cref="IKernelBuilder"/> instance to augment.</param>
    /// <param name="model">The name of the Hugging Face model.</param>
    /// <param name="endpoint">The endpoint URL for the chat completion service.</param>
    /// <param name="apiKey">The API key required for accessing the Hugging Face service.</param>
    /// <param name="serviceId">A local identifier for the given AI service.</param>
    /// <param name="httpClient">The HttpClient to use with this service.</param>
    /// <returns>The same instance as <paramref name="builder"/>.</returns>
    public static IKernelBuilder AddBaiduChatCompletion(
        this IKernelBuilder builder,
        string model,
        Uri endpoint = null,
        string apiKey = null,
        string clientID = null,
        string serviceId = null,
        HttpClient httpClient = null)
    {
        Verify.NotNull(builder);
        Verify.NotNull(model);

        builder.Services.AddKeyedSingleton<IChatCompletionService>(serviceId, (serviceProvider, _) =>
            new BaiduChatCompletionService(
                model,
                endpoint,
                apiKey,
                clientID,
                HttpClientProvider.GetHttpClient(httpClient, serviceProvider),
                serviceProvider.GetService<ILoggerFactory>()
            ));

        return builder;
    }


    public static IKernelBuilder AddBaiduEmbedding(
       this IKernelBuilder builder,
       string modelId,
       Uri endpoint = null,
       string apiKey = null,
       string clientId = null,
       string serviceId = null,
       HttpClient httpClient = null)
    {
        Verify.NotNull(builder);
        Verify.NotNull(modelId);

        builder.Services.AddKeyedSingleton<ITextEmbeddingGenerationService>(serviceId, (serviceProvider, _) =>
            new BaiduTextEmbeddingGenerationService(modelId, endpoint, apiKey, clientId, HttpClientProvider.GetHttpClient(httpClient, serviceProvider), serviceProvider.GetService<ILoggerFactory>()
            ));

        return builder;
    }

}
