using Volo.Abp.Modularity;
using Volo.Abp.Localization;
using Nels.Abp.SysMng.Localization;
using Volo.Abp.Domain;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Validation;
using Volo.Abp.Validation.Localization;
using Volo.Abp.VirtualFileSystem;

namespace Nels.Abp.SysMng;

[DependsOn(
    typeof(AbpValidationModule),
    typeof(AbpDddDomainSharedModule)
)]
public class NelsAbpSysMngDomainSharedModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<NelsAbpSysMngDomainSharedModule>();
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources.Add<SysMngResource>().AddVirtualJson("/Localization/SysMng");
            options.Resources.Add<MenuResource>();
        });

        Configure<AbpExceptionLocalizationOptions>(options =>
        {
            options.MapCodeNamespace("SysMng", typeof(SysMngResource));
            options.MapCodeNamespace("Menu", typeof(MenuResource));
        });
    }
}
