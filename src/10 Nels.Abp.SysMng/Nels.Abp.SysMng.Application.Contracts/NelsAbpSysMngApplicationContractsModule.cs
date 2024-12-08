using Volo.Abp.Application;
using Volo.Abp.Modularity;
using Volo.Abp.Authorization;
using Volo.Abp.Identity;

namespace Nels.Abp.SysMng;

[DependsOn(
    typeof(NelsAbpSysMngDomainSharedModule),
    typeof(AbpDddApplicationContractsModule),
    typeof(AbpIdentityApplicationContractsModule),
    typeof(AbpAuthorizationModule)
    )]
public class NelsAbpSysMngApplicationContractsModule : AbpModule
{

}
