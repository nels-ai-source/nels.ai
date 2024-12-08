using Nels.Abp.SysMng;
using Volo.Abp.Modularity;
using Volo.Abp.ObjectExtending;

namespace Nels.Aigc;

[DependsOn(
    typeof(AigcDomainSharedModule),
    typeof(AbpObjectExtendingModule),
    typeof(NelsAbpSysMngApplicationContractsModule)
)]
public class AigcApplicationContractsModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        AigcDtoExtensions.Configure();
    }
}
