using Volo.Abp.Application;
using Volo.Abp.Modularity;

namespace Nels.Abp.Ddd.Application.Contracts;

[DependsOn(
    typeof(AbpDddApplicationContractsModule)
    )]
public class NelsAbpDddApplicationContractsModule : AbpModule
{
}
