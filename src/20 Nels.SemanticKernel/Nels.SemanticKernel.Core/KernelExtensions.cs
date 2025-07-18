using Microsoft.Extensions.DependencyInjection;
using Microsoft.KernelMemory;
using Microsoft.SemanticKernel;
using Nels.SemanticKernel.Services;
using OpenAI;
using System;
using System.ClientModel;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UglyToad.PdfPig.Tokens;

namespace Nels.SemanticKernel;

public static class KernelExtensions
{
    public static IServiceCollection AddKernelBuilder(this IServiceCollection services, Action<IKernelBuilder>? action = null)
    {
        services.AddScoped(provider =>
        {
            var _kernelBuilder = Kernel.CreateBuilder();

            var modelInstanceService = provider.GetService<IModelService>();
            var modelInstances = modelInstanceService.GetAllModels().GetAwaiter().GetResult();

            var textModelInstances = modelInstances.Where(x => x.Type == Enums.ModelType.TextGeneration).ToList();
            var embeddingInstances = modelInstances.Where(x => x.Type == Enums.ModelType.Embedding).ToList();

            _kernelBuilder.AddChatCompletionServices(textModelInstances).GetAwaiter().GetResult();
            //_kernelBuilder.AddTextEmbeddingGenerations(embeddingInstances).GetAwaiter().GetResult();
            //_kernelBuilder.Services.AddDefaultContentDecoders();

            action?.Invoke(_kernelBuilder);

            return _kernelBuilder.Build();
        });

        services.AddSingleton<AgentActuator>();

        return services;
    }
    public static async Task AddChatCompletionServices(this IKernelBuilder kernelBuilder, List<IModel> models)
    {
        if (models?.Count == 0) return;

        var defaultModel = models.FirstOrDefault();
        if (defaultModel != null)
        {
            await kernelBuilder.AddChatCompletionService(defaultModel, null);
        }
        models.ForEach(async model =>
        {
            await kernelBuilder.AddChatCompletionService(model, model.Id.ToString());
        });

    }
    public static async Task AddChatCompletionService(this IKernelBuilder kernelBuilder, IModel model, string serviceId)
    {
        switch (model.Connector)
        {
            case Enums.ModelConnector.OpenAI:
                if (string.IsNullOrWhiteSpace(model.AccessKey)) return;

                OpenAIClient client = new(new ApiKeyCredential(model.AccessKey), new OpenAIClientOptions
                {
                    Endpoint = new Uri(model.Endpoint)
                });
                kernelBuilder.AddOpenAIChatCompletion(modelId: model.Name, openAIClient: client);
                break;
            case Enums.ModelConnector.AzureOpenAI:
                if (string.IsNullOrWhiteSpace(model.AccessKey)) return;
                kernelBuilder.AddAzureOpenAIChatCompletion(deploymentName: model.DeploymentName, modelId: model.Name, endpoint: model.Endpoint, apiKey: model.AccessKey, serviceId: serviceId);
                break;
            case Enums.ModelConnector.Google:
                if (string.IsNullOrWhiteSpace(model.AccessKey)) return;
                kernelBuilder.AddGoogleAIGeminiChatCompletion(modelId: model.Name, apiKey: model.AccessKey, serviceId: serviceId);
                break;
            case Enums.ModelConnector.HuggingFace:
                if (string.IsNullOrWhiteSpace(model.AccessKey)) return;
                kernelBuilder.AddHuggingFaceChatCompletion(model: model.Name, apiKey: model.AccessKey, serviceId: serviceId);
                break;
            case Enums.ModelConnector.MistralAI:
                if (string.IsNullOrWhiteSpace(model.AccessKey)) return;
                kernelBuilder.AddMistralChatCompletion(modelId: model.Name, endpoint: string.IsNullOrWhiteSpace(model.Endpoint) ? null : new Uri(model.Endpoint), apiKey: model.AccessKey, serviceId: serviceId);
                break;
            case Enums.ModelConnector.Ollama:
                kernelBuilder.AddOllamaChatCompletion(modelId: model.Name, endpoint: string.IsNullOrWhiteSpace(model.Endpoint) ? null : new Uri(model.Endpoint), serviceId: serviceId);
                break;
            default:
                throw new NotImplementedException();
        }
        await Task.CompletedTask;
    }

    public static async Task AddTextEmbeddingGenerations(this IKernelBuilder kernelBuilder, List<IModel> models)
    {
        if (models?.Count == 0) return;

        //var defaultModelInstance = models.FirstOrDefault(x => x.IsDefault);
        //if (defaultModelInstance != null)
        //{
        //    await kernelBuilder.AddTextEmbeddingGeneration(defaultModelInstance, null);
        //}
        models.ForEach(async modelInstance =>
        {
            await kernelBuilder.AddTextEmbeddingGeneration(modelInstance, modelInstance.Id.ToString());
        });

    }
    public static async Task AddTextEmbeddingGeneration(this IKernelBuilder kernelBuilder, IModel model, string serviceId)
    {
        switch (model.Connector)
        {
            case Enums.ModelConnector.AzureOpenAI:
                if (string.IsNullOrWhiteSpace(model.AccessKey)) return;
                kernelBuilder.AddAzureOpenAIEmbeddingGenerator(deploymentName: model.DeploymentName, modelId: model.Name, endpoint: model.Endpoint, apiKey: model.AccessKey, serviceId: serviceId);
                break;
            case Enums.ModelConnector.OpenAI:
                if (string.IsNullOrWhiteSpace(model.AccessKey)) return;
                kernelBuilder.AddOpenAIEmbeddingGenerator(modelId: model.Name, apiKey: model.AccessKey, serviceId: serviceId);
                break;
            case Enums.ModelConnector.Google:
                if (string.IsNullOrWhiteSpace(model.AccessKey)) return;
                kernelBuilder.AddGoogleAIEmbeddingGenerator(modelId: model.Name, apiKey: model.AccessKey, serviceId: serviceId);
                break;
            case Enums.ModelConnector.HuggingFace:
                if (string.IsNullOrWhiteSpace(model.AccessKey)) return;
                kernelBuilder.AddHuggingFaceEmbeddingGenerator(model: model.Name, apiKey: model.AccessKey, serviceId: serviceId);
                break;
            case Enums.ModelConnector.MistralAI:
                if (string.IsNullOrWhiteSpace(model.AccessKey)) return;
                kernelBuilder.AddMistralEmbeddingGenerator(modelId: model.Name, endpoint: string.IsNullOrWhiteSpace(model.Endpoint) ? null : new Uri(model.Endpoint), apiKey: model.AccessKey, serviceId: serviceId);
                break;
            case Enums.ModelConnector.Ollama:
                kernelBuilder.AddOllamaEmbeddingGenerator(modelId: model.Name, endpoint: string.IsNullOrWhiteSpace(model.Endpoint) ? null : new Uri(model.Endpoint), serviceId: serviceId);
                break;
            default:
                throw new NotImplementedException();
        }
        await Task.CompletedTask;
    }
}
