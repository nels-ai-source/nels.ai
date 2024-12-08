using Nels.Aigc;
using Nels.Aigc.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace Nels.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(AigcEntityFrameworkCoreModule),
    typeof(AigcApplicationContractsModule)
    )]
public class DbMigratorModule : AbpModule
{
}
