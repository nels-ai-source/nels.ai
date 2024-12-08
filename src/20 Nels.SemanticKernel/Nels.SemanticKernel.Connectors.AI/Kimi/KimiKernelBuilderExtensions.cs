using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Nels.SemanticKernel.InternalUtilities.Http;
using Nels.SemanticKernel.Kimi.Services;
using System;
using System.Net.Http;

namespace Nels.SemanticKernel.Kimi;

public static class KimiKernelBuilderExtensions
{
    public static IKernelBuilder AddKimiChatCompletion(this IKernelBuilder builder,
        string modelId,
        Uri endpoint = null,
        string apiKey = null,
        string serviceId = null,
        HttpClient httpClient = null)
    {
        Verify.NotNull(builder);
        Verify.NotNull(modelId);

        builder.Services.AddKeyedSingleton<IChatCompletionService>(serviceId, (serviceProvider, _) =>
            new KimiChatCompletionService(
                modelId,
                endpoint,
                apiKey,
                HttpClientProvider.GetHttpClient(httpClient, serviceProvider),
                serviceProvider.GetService<ILoggerFactory>()
            ));

        return builder;
    }
}
