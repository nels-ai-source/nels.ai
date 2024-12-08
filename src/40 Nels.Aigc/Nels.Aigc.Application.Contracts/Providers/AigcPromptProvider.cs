using Nels.Aigc.Entities;
using Nels.SemanticKernel.Template;
using System;
using Volo.Abp.Guids;
using Volo.Abp.Localization;

namespace Nels.Aigc.Providers;

public class AigcPromptProvider(IGuidGenerator guidGenerator, ILocalizableStringSerializer localizableStringSerializer) : PromptProvider
{
    public IGuidGenerator GuidGenerator = guidGenerator;
    public ILocalizableStringSerializer LocalizableStringSerializer { get; } = localizableStringSerializer;

    public override void Define(IPromptDefinitionContext context)
    {
        var hiPrompt = new Prompt(Guid.Parse("20a59e55-0e05-4863-8a7c-aae4e3fabae4"))
        {
            Name = "Hi",
            Template = "Hi",
            TemplateFormat = TemplateFormatConstant.SemanticKernel
        };
        context.AddPrompt(hiPrompt);
    }
}