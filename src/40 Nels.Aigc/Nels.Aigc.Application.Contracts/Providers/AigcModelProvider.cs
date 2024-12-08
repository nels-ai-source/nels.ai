using Nels.Aigc.Entities;
using Nels.SemanticKernel.Enums;
using System;
using Volo.Abp.Guids;
using Volo.Abp.Localization;

namespace Nels.Aigc.Providers;

public class AigcModelProvider(IGuidGenerator guidGenerator, ILocalizableStringSerializer localizableStringSerializer) : ModelProvider
{
    public IGuidGenerator GuidGenerator = guidGenerator;
    public ILocalizableStringSerializer LocalizableStringSerializer { get; } = localizableStringSerializer;

    public override void Define(IModelDefinitionContext context)
    {
        var accessKey = "AccessKey";
        var deploymentName = "AccessKey|Endpoint|DeploymentName";
        #region OpenAI
        context.AddModel(new Model(Guid.Parse("ebcaebb0-aa21-45a0-9bae-f28149f4d20e"))
        {
            Name = "gpt-3.5-turbo",
            Properties = accessKey,
            Provider = SemanticKernel.Enums.ModelProvider.OpenAI,
            Type = ModelType.TextGeneration,
        });
        context.AddModel(new Model(Guid.Parse("95d30ca4-70b5-48a5-a02e-405ccf28f57e"))
        {
            Name = "gpt-4",
            Properties = accessKey,
            Provider = SemanticKernel.Enums.ModelProvider.OpenAI,
            Type = ModelType.TextGeneration,
        });
        context.AddModel(new Model(Guid.Parse("6fc5ae4f-5d7e-4973-97e7-d49eaa2105d6"))
        {
            Name = "gpt-4-turbo",
            Properties = accessKey,
            Provider = SemanticKernel.Enums.ModelProvider.OpenAI,
            Type = ModelType.TextGeneration,
        });
        context.AddModel(new Model(Guid.Parse("c631b1e9-115d-451a-b8a4-e7737f8f4264"))
        {
            Name = "o1-mini",
            Properties = accessKey,
            Provider = SemanticKernel.Enums.ModelProvider.OpenAI,
            Type = ModelType.TextGeneration,
        });
        context.AddModel(new Model(Guid.Parse("51d16d24-42c3-4042-9b7e-621efb954c70"))
        {
            Name = "o1-preview",
            Properties = accessKey,
            Provider = SemanticKernel.Enums.ModelProvider.OpenAI,
            Type = ModelType.TextGeneration,
        });
        context.AddModel(new Model(Guid.Parse("9cc8b078-836b-4523-9352-35d5f3319f74"))
        {
            Name = "gpt-4o",
            Properties = accessKey,
            Provider = SemanticKernel.Enums.ModelProvider.OpenAI,
            Type = ModelType.TextGeneration,
        });
        context.AddModel(new Model(Guid.Parse("9d0b1adf-3591-4522-b3d1-6659491b209c"))
        {
            Name = "gpt-4o-mini",
            Properties = accessKey,
            Provider = SemanticKernel.Enums.ModelProvider.OpenAI,
            Type = ModelType.TextGeneration,
        });

        context.AddModel(new Model(Guid.Parse("d9873e95-e10c-4944-8435-eca343ae3ac0"))
        {
            Name = "text-embedding-ada-002",
            Properties = accessKey,
            Provider = SemanticKernel.Enums.ModelProvider.OpenAI,
            Type = ModelType.Embedding,
        });
        context.AddModel(new Model(Guid.Parse("c8ac0f58-f219-456f-8d3a-4b7542b9bd46"))
        {
            Name = "text-embedding-3-small",
            Properties = accessKey,
            Provider = SemanticKernel.Enums.ModelProvider.OpenAI,
            Type = ModelType.Embedding,
        });
        context.AddModel(new Model(Guid.Parse("a412a286-0553-405d-a1b7-15bf2b311738"))
        {
            Name = "text-embedding-3-large",
            Properties = accessKey,
            Provider = SemanticKernel.Enums.ModelProvider.OpenAI,
            Type = ModelType.Embedding,
        });
        #endregion

        #region AzureOpenAI

        context.AddModel(new Model(Guid.Parse("b4266197-3687-407d-ab68-b7f01622148c"))
        {
            Name = "gpt-3.5-turbo",
            Properties = deploymentName,
            Provider = SemanticKernel.Enums.ModelProvider.AzureOpenAI,
            Type = ModelType.TextGeneration,
        });
        context.AddModel(new Model(Guid.Parse("31af4a61-e5df-432f-a277-163646fdd7db"))
        {
            Name = "gpt-4",
            Properties = deploymentName,
            Provider = SemanticKernel.Enums.ModelProvider.AzureOpenAI,
            Type = ModelType.TextGeneration,
        });
        context.AddModel(new Model(Guid.Parse("b244eff4-1fad-4b7c-aca7-4f4d5b51e964"))
        {
            Name = "gpt-4-turbo",
            Properties = deploymentName,
            Provider = SemanticKernel.Enums.ModelProvider.AzureOpenAI,
            Type = ModelType.TextGeneration,
        });
        context.AddModel(new Model(Guid.Parse("051393cb-9cba-4c7f-80f5-4e3b2bd3b8ee"))
        {
            Name = "o1-mini",
            Properties = deploymentName,
            Provider = SemanticKernel.Enums.ModelProvider.AzureOpenAI,
            Type = ModelType.TextGeneration,
        });
        context.AddModel(new Model(Guid.Parse("e716a340-86e0-4285-924c-98ec2275b795"))
        {
            Name = "o1-preview",
            Properties = deploymentName,
            Provider = SemanticKernel.Enums.ModelProvider.AzureOpenAI,
            Type = ModelType.TextGeneration,
        });
        context.AddModel(new Model(Guid.Parse("f69140aa-b03f-41b8-9e3f-028e9bc396b8\r\n"))
        {
            Name = "gpt-4o",
            Properties = deploymentName,
            Provider = SemanticKernel.Enums.ModelProvider.AzureOpenAI,
            Type = ModelType.TextGeneration,
        });
        context.AddModel(new Model(Guid.Parse("c61e45bd-1508-4f10-b37f-982423d5a740"))
        {
            Name = "gpt-4o-mini",
            Properties = deploymentName,
            Provider = SemanticKernel.Enums.ModelProvider.AzureOpenAI,
            Type = ModelType.TextGeneration,
        });

        context.AddModel(new Model(Guid.Parse("1edd7415-dcaf-4429-b372-6974c981ac02"))
        {
            Name = "text-embedding-ada-002",
            Properties = deploymentName,
            Provider = SemanticKernel.Enums.ModelProvider.AzureOpenAI,
            Type = ModelType.Embedding,
        });
        context.AddModel(new Model(Guid.Parse("9e203bc8-4b55-45fa-9b5b-c3babec18c5a"))
        {
            Name = "text-embedding-3-small",
            Properties = deploymentName,
            Provider = SemanticKernel.Enums.ModelProvider.AzureOpenAI,
            Type = ModelType.Embedding,
        });
        context.AddModel(new Model(Guid.Parse("f7dc9072-f8c7-4df2-abc9-095aa1f76424"))
        {
            Name = "text-embedding-3-large",
            Properties = deploymentName,
            Provider = SemanticKernel.Enums.ModelProvider.AzureOpenAI,
            Type = ModelType.Embedding,
        });
        #endregion

        #region dashScope
        var dashScopeEndpont = "https://dashscope.aliyuncs.com";
        var qwenTurbo = new Model(Guid.Parse("56915bde-f815-4ab1-85ad-b229024e09e4"))
        {
            Name = "qwen-turbo",
            Endpoint = dashScopeEndpont,
            Properties = accessKey,
            Provider = SemanticKernel.Enums.ModelProvider.DashScope,
            Type = ModelType.TextGeneration,
        };
        context.AddModel(qwenTurbo);

        var qwenPlus = new Model(Guid.Parse("f8e0e85a-4ccd-4e8d-9693-3095305b17bb"))
        {
            Name = "qwen-plus",
            Endpoint = dashScopeEndpont,
            Properties = accessKey,
            Provider = SemanticKernel.Enums.ModelProvider.DashScope,
            Type = ModelType.TextGeneration
        };
        context.AddModel(qwenPlus);

        var qwenMax = new Model(Guid.Parse("11c493ce-5d89-4f1c-a056-a63dbc25583d"))
        {
            Name = "qwen-max",
            Endpoint = dashScopeEndpont,
            Properties = accessKey,
            Provider = SemanticKernel.Enums.ModelProvider.DashScope,
            Type = ModelType.TextGeneration
        };
        context.AddModel(qwenMax);

        var textEmbeddingv2 = new Model(Guid.Parse("32e63129-fb1b-4d83-b65c-acf4a90923fb"))
        {
            Name = "text-embedding-v2",
            Endpoint = dashScopeEndpont,
            Properties = accessKey,
            Provider = SemanticKernel.Enums.ModelProvider.DashScope,
            Type = ModelType.Embedding
        };
        context.AddModel(textEmbeddingv2);
        #endregion

        #region deepseek
        var deepseekEndpont = "https://api.deepseek.com";
        var deepseekChat = new Model(Guid.Parse("610862a6-d1fd-4c04-897c-6cd5cba0eb12"))
        {
            Name = "deepseek-chat",
            Endpoint = deepseekEndpont,
            Properties = accessKey,
            Provider = SemanticKernel.Enums.ModelProvider.DeepSeek,
            Type = ModelType.TextGeneration
        };
        context.AddModel(deepseekChat);

        var deepseekCoder = new Model(Guid.Parse("81f41a7b-1a40-460a-bc9f-bdd8125a7778"))
        {
            Name = "deepseek-coder",
            Endpoint = deepseekEndpont,
            Properties = accessKey,
            Provider = SemanticKernel.Enums.ModelProvider.DeepSeek,
            Type = ModelType.TextGeneration
        };
        context.AddModel(deepseekCoder);
        #endregion


    }

}
