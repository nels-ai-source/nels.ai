using Nels.Abp.SysMng;
using Nels.Abp.SysMng.Localization;
using Nels.Aigc.Localization;
using Volo.Abp.AuditLogging;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity;
using Volo.Abp.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Modularity;
using Volo.Abp.OpenIddict;
using Volo.Abp.PermissionManagement;
using Volo.Abp.SettingManagement;
using Volo.Abp.TenantManagement;
using Volo.Abp.Validation.Localization;
using Volo.Abp.VirtualFileSystem;

namespace Nels.Aigc;
[DependsOn(
    typeof(AbpAuditLoggingDomainSharedModule),
    typeof(AbpBackgroundJobsDomainSharedModule),
    typeof(AbpFeatureManagementDomainSharedModule),
    typeof(AbpIdentityDomainSharedModule),
    typeof(AbpOpenIddictDomainSharedModule),
    typeof(AbpPermissionManagementDomainSharedModule),
    typeof(AbpSettingManagementDomainSharedModule),
    typeof(AbpTenantManagementDomainSharedModule),
    typeof(NelsAbpSysMngDomainSharedModule)
    )]
public class AigcDomainSharedModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        AigcGlobalFeatureConfigurator.Configure();
        AigcModuleExtensionConfigurator.Configure();
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure(delegate (AbpVirtualFileSystemOptions options)
        {
            options.FileSets.AddEmbedded<AigcDomainSharedModule>();
        });

        Configure(delegate (AbpLocalizationOptions options)
        {
            options.Resources.Add<AigcResource>("en").AddVirtualJson("/Localization/Aigc");
            options.Resources.Get<MenuResource>().AddVirtualJson("/Localization/AigcMenu");
        });

        Configure(delegate (AbpExceptionLocalizationOptions options)
        {
            options.MapCodeNamespace("Aigc", typeof(AigcResource));
        });
    }
}
