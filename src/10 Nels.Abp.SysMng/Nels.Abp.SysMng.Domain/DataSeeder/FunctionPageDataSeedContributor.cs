using Microsoft.Extensions.Options;
using Nels.Abp.SysMng.FunctionPage;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;

namespace Nels.Abp.SysMng.DataSeeder;

public class FunctionPageDataSeedContributor(
    IPermissionDefinitionManager permissionDefinitionManager,
    IOptions<FunctionPageOptions> options,
    IServiceProvider services,
    AppDomainService appDomainService,
    ICurrentTenant currentTenant) : IDataSeedContributor, ITransientDependency
{
    protected ICurrentTenant CurrentTenant { get; } = currentTenant;
    protected IOptions<FunctionPageOptions> Options { get; } = options;
    protected IPermissionDefinitionManager PermissionDefinitionManager { get; } = permissionDefinitionManager;
    protected IServiceProvider ServiceProvider { get; } = services;
    protected AppDomainService AppDomainService { get; } = appDomainService;

    public async Task SeedAsync(DataSeedContext context)
    {
        var definitionContext = new FunctionPageDefinitionContext(ServiceProvider);

        var providers = Options.Value
        .DefinitionProviders
            .Select(p => ServiceProvider.GetService(p) as IFunctionPageProvider)
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

        if (AppDomainService == null) return;
        foreach (var appGroup in definitionContext.AppGroups)
        {
            await AppDomainService.InitAppPagesAsync(appGroup.Value.App, appGroup.Value.BusinessUnits.ToList(), appGroup.Value.Page.ToList());
        }
    }
}
