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

public class ModelDataSeedContributor(
    IOptions<ModelOptions> options,
    IServiceProvider services,
    IRepository<Model, Guid> repository,
    IRepository<ModelInstance, Guid> modelInstanceRrepository,
    ICurrentTenant currentTenant) : IDataSeedContributor, ITransientDependency
{
    protected ICurrentTenant CurrentTenant { get; } = currentTenant;
    protected IOptions<ModelOptions> Options { get; } = options;
    protected IServiceProvider ServiceProvider { get; } = services;

    public async Task SeedAsync(DataSeedContext context)
    {
        var definitionContext = new ModelDefinitionContext(ServiceProvider);

        var providers = Options.Value
        .DefinitionProviders
            .Select(p => ServiceProvider.GetService(p) as IModelProvider)
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

        if (repository == null || definitionContext.Models == null) return;


        if (definitionContext.Models.Count != 0)
        {
            var modelIds = definitionContext.Models.Select(p => p.Id).ToList();
            await repository.DeleteAsync(x => modelIds.Contains(x.Id));
            await repository.InsertManyAsync(definitionContext.Models);
        }

        if (definitionContext.ModelInstances.Count != 0)
        {
            var modelInstanceIds = definitionContext.ModelInstances.Select(p => p.Id).ToList();
            await modelInstanceRrepository.DeleteAsync(x => modelInstanceIds.Contains(x.Id));
            await modelInstanceRrepository.InsertManyAsync(definitionContext.ModelInstances);
        }
    }
}
