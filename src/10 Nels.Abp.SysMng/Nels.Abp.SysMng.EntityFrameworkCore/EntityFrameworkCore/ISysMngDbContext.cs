using Microsoft.EntityFrameworkCore;
using Nels.Abp.SysMng.Files;
using Nels.Abp.SysMng.FunctionPage;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.TenantManagement.EntityFrameworkCore;

namespace Nels.Abp.SysMng.EntityFrameworkCore;

[ConnectionStringName(SysMngDbProperties.ConnectionStringName)]
public interface ISysMngDbContext : IIdentityDbContext, ITenantManagementDbContext, IEfCoreDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * DbSet<Question> Questions { get; }
     */
    public DbSet<App> Apps { get; set; }
    public DbSet<BusinessUnit> BusinessUnits { get; set; }
    public DbSet<Page> Pages { get; set; }

    public DbSet<BizParameter> BizParameters { get; set; }
    public DbSet<BizParameterValue> BizParameterValues { get; set; }
    public DbSet<FileEntity> FileEntities { get; set; }
}
