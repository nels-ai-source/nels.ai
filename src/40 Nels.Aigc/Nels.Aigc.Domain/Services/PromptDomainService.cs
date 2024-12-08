using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.PromptTemplates.Handlebars;
using Microsoft.SemanticKernel.PromptTemplates.Liquid;
using Nels.Aigc.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace Nels.Aigc.Services;

public class PromptDomainService(IRepository<Prompt, Guid> repository
    , IKernelBuilder kernelBuilder) : DomainService
{
    public async Task<ChatHistory> GetChatMessages(Guid promptGuid, KernelArguments arguments)
    {
        var prompt = await repository.FirstOrDefaultAsync(x => x.Id == promptGuid);
        if (prompt == null) return [];

        var factory = await CreatePromptTemplateFactory(prompt.TemplateFormat);
        var kenel = kernelBuilder.Build();

        var chatHistory = new ChatHistory(await RenderTemplate(prompt.Template, prompt.TemplateFormat, factory, kenel, arguments));

        return await Task.FromResult(chatHistory);
    }

    private async Task<string> RenderTemplate(string template, string templateFormat, IPromptTemplateFactory factory, Kernel kernel, KernelArguments arguments)
    {
        var promptTemplateConfig = new PromptTemplateConfig()
        {
            Template = template,
            TemplateFormat = templateFormat
        };
        var promptTemplate = factory.Create(promptTemplateConfig);
        return await promptTemplate.RenderAsync(kernel, arguments);

    }

    private async Task<IPromptTemplateFactory> CreatePromptTemplateFactory(string templateFormat)
    {
        if (templateFormat == LiquidPromptTemplateFactory.LiquidTemplateFormat)
        {
            return await Task.FromResult(new LiquidPromptTemplateFactory());
        }
        if (templateFormat == HandlebarsPromptTemplateFactory.HandlebarsTemplateFormat)
        {
            return await Task.FromResult(new HandlebarsPromptTemplateFactory());
        }
        return await Task.FromResult(new KernelPromptTemplateFactory());
    }
}
