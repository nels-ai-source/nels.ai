// Copyright (c) Microsoft. All rights reserved.

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Embeddings;
using Nels.SemanticKernel.DeepSeek.Services;
using Nels.SemanticKernel.InternalUtilities.Http;
using System;
using System.Net.Http;

namespace Nels.SemanticKernel.DeepSeek;

/// <summary>
/// Provides extension methods for the <see cref="IKernelBuilder"/> class to configure Hugging Face connectors.
/// </summary>
public static class DeepSeekKernelBuilderExtensions
{

    /// <summary>
    /// Adds an DashScope chat completion service with the specified configuration.
    /// </summary>
    /// <param name="builder">The <see cref="IKernelBuilder"/> instance to augment.</param>
    /// <param name="modelId">The name of the DashScope model.</param>
    /// <param name="endpoint">The endpoint URL for the chat completion service.</param>
    /// <param name="apiKey">The API key required for accessing the Hugging Face service.</param>
    /// <param name="serviceId">A local identifier for the given AI service.</param>
    /// <param name="httpClient">The HttpClient to use with this service.</param>
    /// <returns>The same instance as <paramref name="builder"/>.</returns>
    public static IKernelBuilder AddDeepSeekChatCompletion(
        this IKernelBuilder builder,
        string modelId,
        Uri endpoint = null,
        string apiKey = null,
        string serviceId = null,
        HttpClient httpClient = null)
    {
        Verify.NotNull(builder);
        Verify.NotNull(modelId);

        builder.Services.AddKeyedSingleton<IChatCompletionService>(serviceId, (serviceProvider, _) =>
            new DeepSeekChatCompletionService(
                modelId,
                endpoint,
                apiKey,
                HttpClientProvider.GetHttpClient(httpClient, serviceProvider),
                serviceProvider.GetService<ILoggerFactory>()
            ));

        return builder;
    }

    public static IKernelBuilder AddDeepSeekTextEmbeddingGeneration(
    this IKernelBuilder builder,
    string modelId,
    Uri endpoint = null,
    string apiKey = null,
    string serviceId = null,
    HttpClient httpClient = null)
    {
        Verify.NotNull(builder);
        Verify.NotNull(modelId);

        builder.Services.AddKeyedSingleton<ITextEmbeddingGenerationService>(serviceId, (serviceProvider, _) =>
            new DeepSeekTextEmbeddingGenerationService(
                modelId,
                endpoint,
                apiKey,
                HttpClientProvider.GetHttpClient(httpClient, serviceProvider),
                serviceProvider.GetService<ILoggerFactory>()
            ));

        return builder;
    }
}
