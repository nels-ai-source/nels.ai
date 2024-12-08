using Microsoft.Extensions.DependencyInjection;
using Nels.Abp.Ddd.Application;
using Volo.Abp.AutoMapper;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;

namespace Nels.Abp.SysMng;

[DependsOn(
    typeof(NelsAbpSysMngDomainModule),
    typeof(NelsAbpSysMngApplicationContractsModule),
    typeof(NelsAbpDddApplicationModule),
    typeof(AbpIdentityApplicationModule),
    typeof(AbpAutoMapperModule)
    )]
public class NelsAbpSysMngApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAutoMapperObjectMapper<NelsAbpSysMngApplicationModule>();
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<NelsAbpSysMngApplicationModule>(validate: true);
        });
    }
}
