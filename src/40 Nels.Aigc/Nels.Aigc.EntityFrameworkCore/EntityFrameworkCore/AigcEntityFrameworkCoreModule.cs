using Microsoft.Extensions.DependencyInjection;
using Nels.Abp.SysMng;
using Nels.Abp.SysMng.EntityFrameworkCore;
using System;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.PostgreSql;
using Microsoft.EntityFrameworkCore;
using Nels.Aigc.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.Modularity;
using Volo.Abp.EntityFrameworkCore.DependencyInjection;

namespace Nels.Aigc.EntityFrameworkCore;

[DependsOn(
    typeof(AigcDomainModule),
    typeof(AbpEntityFrameworkCorePostgreSqlModule),
    typeof(NelsAbpSysMngEntityFrameworkCoreModule)
    )]
public class AigcEntityFrameworkCoreModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        // https://www.npgsql.org/efcore/release-notes/6.0.html#opting-out-of-the-new-timestamp-mapping-logic
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        AigcEfCoreEntityExtensionMappings.Configure();
        HostedServiceExtensions.SetDbTablePrefix();
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<AigcDbContext>(options =>
        {
            /* Remove "includeAllEntities: true" to create
             * default repositories only for aggregate roots */
            options.AddDefaultRepositories(includeAllEntities: true);
        });

        Configure<AbpDbContextOptions>(options =>
        {
            /* The main point to change your DBMS.
             * See also AigcMigrationsDbContextFactory for EF Core tooling. */
            options.UseNpgsql();
        });

        Configure<AbpEntityOptions>(options =>
        {
            options.Entity<Agent>(orderOptions =>
            {
                orderOptions.DefaultWithDetailsFunc = query => query.Include(x => x.Questions).Include(x => x.Knowledges).Include(x => x.Tools).Include(x => x.KnowledgeOption);
            });
        });
    }
}
