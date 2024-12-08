using System;
using System.Collections.Generic;
using System.Linq;
using Nels.Aigc.Entities;
using Volo.Abp;

namespace Nels.Aigc.Providers;

public class PromptDefinitionContext(IServiceProvider serviceProvider) : IPromptDefinitionContext
{
    public IServiceProvider ServiceProvider { get; } = serviceProvider;
    public List<Prompt> Prompts { get; } = [];

    public void AddPrompt(Prompt prompt)
    {
        Check.NotNull(prompt, nameof(prompt));

        if (Prompts.Any(x => x.Id == prompt.Id))
        {
            throw new AbpException($"There is already an existing prompt with name: {prompt.Id}");
        }
        Prompts.Add(prompt);
    }
}
