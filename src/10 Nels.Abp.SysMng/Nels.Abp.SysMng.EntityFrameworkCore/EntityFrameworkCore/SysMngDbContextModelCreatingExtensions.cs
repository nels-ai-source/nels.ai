using Microsoft.EntityFrameworkCore;
using Nels.Abp.SysMng.FunctionPage;
using Volo.Abp;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.OpenIddict.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.TenantManagement.EntityFrameworkCore;

namespace Nels.Abp.SysMng.EntityFrameworkCore;

public static class SysMngDbContextModelCreatingExtensions
{

    public static void ConfigureSysMng(
        this ModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));

        if (builder.IsTenantOnlyDatabase())
        {
            return;
        }

        AbpCommonDbProperties.DbTablePrefix = SysMngDbProperties.DbTablePrefix;

        builder.ConfigurePermissionManagement();
        builder.ConfigureSettingManagement();
        builder.ConfigureBackgroundJobs();
        builder.ConfigureAuditLogging();
        builder.ConfigureIdentity();
        builder.ConfigureOpenIddict();
        builder.ConfigureFeatureManagement();
        builder.ConfigureTenantManagement();

        #region functionPage
        builder.Entity<App>(b =>
        {
            b.ToTable(SysMngDbProperties.DbTablePrefix + nameof(App), SysMngDbProperties.DbSchema);

            b.ConfigureByConvention();
        });
        builder.Entity<BusinessUnit>(b =>
        {
            b.ToTable(SysMngDbProperties.DbTablePrefix + nameof(BusinessUnit), SysMngDbProperties.DbSchema);

            b.ConfigureByConvention();
        });

        builder.Entity<Page>(b =>
        {
            b.ToTable(SysMngDbProperties.DbTablePrefix + nameof(Page), SysMngDbProperties.DbSchema);

            b.ConfigureByConvention();

        });

        builder.Entity<BizParameter>(b =>
        {
            b.ToTable(SysMngDbProperties.DbTablePrefix + nameof(BizParameter), SysMngDbProperties.DbSchema);

            b.ConfigureByConvention();
        });

        builder.Entity<BizParameterValue>(b =>
        {
            b.ToTable(SysMngDbProperties.DbTablePrefix + nameof(BizParameterValue), SysMngDbProperties.DbSchema);

            b.ConfigureByConvention();
        });
        #endregion



    }
}
