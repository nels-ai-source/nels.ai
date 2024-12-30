using Microsoft.Extensions.DependencyInjection;
using Nels.Abp.SysMng.FunctionPage;
using System;
using System.Collections.Generic;
using System.IO;
using Volo.Abp.BlobStoring;
using Volo.Abp.BlobStoring.FileSystem;
using Volo.Abp.Data;
using Volo.Abp.Domain;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;

namespace Nels.Abp.SysMng;

[DependsOn(
    typeof(AbpBlobStoringFileSystemModule),
    typeof(AbpDddDomainModule),
    typeof(AbpIdentityDomainModule),
    typeof(NelsAbpSysMngDomainSharedModule)
)]
public class NelsAbpSysMngDomainModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        AutoAddDefinitionProviders(context.Services);

        AbpCommonDbProperties.DbTablePrefix = SysMngDbProperties.DbTablePrefix;
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<PermissionManagementOptions>(options =>
        {
            options.SaveStaticPermissionsToDatabase = false;
            options.IsDynamicPermissionStoreEnabled = false;
        });
    //https://abp.io/docs/latest/framework/infrastructure/blob-storing#blob-storage-providers
        Configure<AbpBlobStoringOptions>(options =>
        {
            options.Containers.ConfigureDefault(container =>
            {
                container.UseFileSystem(fileSystem =>
                {
                    var basePath = Path.Combine(AppContext.BaseDirectory, "files");
                    if (!Directory.Exists(basePath))
                    {
                        Directory.CreateDirectory(basePath);
                    }
                    fileSystem.BasePath = basePath;
                });
            });
        });
    }
    #region functionPage
    private static void AutoAddDefinitionProviders(IServiceCollection services)
    {
        var definitionProviders = new List<Type>();

        services.OnRegistered(context =>
        {
            if (typeof(IFunctionPageProvider).IsAssignableFrom(context.ImplementationType))
            {
                definitionProviders.Add(context.ImplementationType);
            }
        });

        services.Configure<FunctionPageOptions>(options =>
        {
            options.DefinitionProviders.AddIfNotContains(definitionProviders);
        });
    }
    #endregion
}
