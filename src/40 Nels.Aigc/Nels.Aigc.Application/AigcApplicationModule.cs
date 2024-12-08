using Nels.Abp.SysMng;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace Nels.Aigc;

[DependsOn(
    typeof(AigcDomainModule),
    typeof(AigcApplicationContractsModule),
    typeof(NelsAbpSysMngApplicationModule)
    )]
public class AigcApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<AigcApplicationModule>();
        });
    }
}
