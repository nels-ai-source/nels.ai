using Microsoft.Extensions.Options;
using Nels.Aigc.Entities;
using Nels.Aigc.Providers;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.MultiTenancy;

namespace Nels.Aigc.DataSeeder;

public class PromptDataSeedContributor(
    IOptions<PromptOptions> options,
    IServiceProvider services,
    IRepository<Prompt, Guid> repository,
    ICurrentTenant currentTenant) : IDataSeedContributor, ITransientDependency
{
    protected ICurrentTenant CurrentTenant { get; } = currentTenant;
    protected IOptions<PromptOptions> Options { get; } = options;
    protected IServiceProvider ServiceProvider { get; } = services;

    public async Task SeedAsync(DataSeedContext context)
    {
        var definitionContext = new PromptDefinitionContext(ServiceProvider);

        var providers = Options.Value
        .DefinitionProviders
            .Select(p => ServiceProvider.GetService(p) as IPromptProvider)
            .ToList();

        foreach (var provider in providers)
        {
            provider?.PreDefine(definitionContext);
        }

        foreach (var provider in providers)
        {
            provider?.Define(definitionContext);
        }

        foreach (var provider in providers)
        {
            provider?.PostDefine(definitionContext);
        }

        if (repository == null || definitionContext.Prompts?.Count == 0) return;

        await repository.DeleteAsync(x => true);
        if (definitionContext?.Prompts?.Count != 0)
        {
            await repository.InsertManyAsync(definitionContext.Prompts);
        }
    }
}
