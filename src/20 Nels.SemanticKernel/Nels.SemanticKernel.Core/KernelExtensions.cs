using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using Nels.SemanticKernel.DeepSeek;
using Nels.SemanticKernel.Kimi;
using Nels.SemanticKernel.DashScope;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nels.SemanticKernel;

public static class KernelExtensions
{
    public static IServiceCollection AddKernelBuilder(this IServiceCollection services, Action<IKernelBuilder>? action = null)
    {
        services.AddScoped(provider =>
        {
            var _kernelBuilder = Kernel.CreateBuilder();

            var modelInstanceService = provider.GetService<IModelInstanceService>();
            var modelInstances = modelInstanceService.GetAllInstances().GetAwaiter().GetResult();

            var textModelInstances = modelInstances.Where(x => x.Type == Enums.ModelType.TextGeneration).ToList();
            var embeddingInstances = modelInstances.Where(x => x.Type == Enums.ModelType.Embedding).ToList();

            _kernelBuilder.AddChatCompletionServices(textModelInstances).GetAwaiter().GetResult();
            _kernelBuilder.AddTextEmbeddingGenerations(embeddingInstances).GetAwaiter().GetResult();

            action?.Invoke(_kernelBuilder);

            return _kernelBuilder.Build();
        });

        return services;
    }
    public static async Task AddChatCompletionServices(this IKernelBuilder kernelBuilder, List<IModelInstance> modelInstances)
    {
        if (modelInstances?.Count == 0) return;

        var defaultModelInstance = modelInstances.FirstOrDefault(x => x.IsDefault);
        if (defaultModelInstance != null)
        {
            await kernelBuilder.AddChatCompletionService(defaultModelInstance, null);
        }
        modelInstances.ForEach(async modelInstance =>
        {
            await kernelBuilder.AddChatCompletionService(modelInstance, modelInstance.Id.ToString());
        });

    }
    public static async Task AddChatCompletionService(this IKernelBuilder kernelBuilder, IModelInstance modelInstance, string serviceId)
    {
        switch (modelInstance.Provider)
        {
            case Enums.ModelProvider.AzureOpenAI:
                kernelBuilder.AddAzureOpenAIChatCompletion(deploymentName: modelInstance.DeploymentName, modelId: modelInstance.Name, endpoint: modelInstance.Endpoint, apiKey: modelInstance.AccessKey, serviceId: serviceId);
                break;
            case Enums.ModelProvider.OpenAI:
                kernelBuilder.AddOpenAIChatCompletion(modelId: modelInstance.Name, apiKey: modelInstance.AccessKey, serviceId: serviceId);
                break;
            case Enums.ModelProvider.DeepSeek:
                kernelBuilder.AddDeepSeekChatCompletion(modelId: modelInstance.Name, endpoint: new Uri(modelInstance.Endpoint), apiKey: modelInstance.AccessKey, serviceId: serviceId);
                break;
            case Enums.ModelProvider.Kimi:
                kernelBuilder.AddKimiChatCompletion(modelId: modelInstance.Name, endpoint: new Uri(modelInstance.Endpoint), apiKey: modelInstance.AccessKey, serviceId: serviceId);
                break;
            case Enums.ModelProvider.DashScope:
                kernelBuilder.AddDashScopeChatCompletion(modelId: modelInstance.Name, endpoint: new Uri(modelInstance.Endpoint), apiKey: modelInstance.AccessKey, serviceId: serviceId);
                break;
            default:
                break;
        }
        await Task.CompletedTask;
    }

    public static async Task AddTextEmbeddingGenerations(this IKernelBuilder kernelBuilder, List<IModelInstance> modelInstances)
    {
        if (modelInstances?.Count == 0) return;

        var defaultModelInstance = modelInstances.FirstOrDefault(x => x.IsDefault);
        if (defaultModelInstance != null)
        {
            await kernelBuilder.AddTextEmbeddingGeneration(defaultModelInstance, null);
        }
        modelInstances.ForEach(async modelInstance =>
        {
            await kernelBuilder.AddTextEmbeddingGeneration(modelInstance, modelInstance.Id.ToString());
        });

    }
    public static async Task AddTextEmbeddingGeneration(this IKernelBuilder kernelBuilder, IModelInstance modelInstance, string serviceId)
    {
        switch (modelInstance.Provider)
        {
            case Enums.ModelProvider.AzureOpenAI:
                kernelBuilder.AddAzureOpenAITextEmbeddingGeneration(deploymentName: modelInstance.DeploymentName, modelId: modelInstance.Name, endpoint: modelInstance.Endpoint, apiKey: modelInstance.SecretKey, serviceId: serviceId);
                break;
            case Enums.ModelProvider.OpenAI:
                kernelBuilder.AddOpenAITextEmbeddingGeneration(modelId: modelInstance.Name, apiKey: modelInstance.SecretKey, serviceId: serviceId);
                break;
            case Enums.ModelProvider.DashScope:
                kernelBuilder.AddDeepSeekTextEmbeddingGeneration(modelId: modelInstance.Name, endpoint: new Uri(modelInstance.Endpoint), apiKey: modelInstance.SecretKey, serviceId: serviceId);
                break;
            default:
                break;
        }
        await Task.CompletedTask;
    }
}
