using Volo.Abp.Application;
using Volo.Abp.Modularity;

namespace Nels.Abp.Ddd.Application;

[DependsOn(
    typeof(AbpDddApplicationModule)
    )]
public class NelsAbpDddApplicationModule : AbpModule
{
}
