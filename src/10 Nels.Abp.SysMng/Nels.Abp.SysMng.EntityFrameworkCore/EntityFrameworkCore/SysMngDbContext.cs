using Microsoft.EntityFrameworkCore;
using Nels.Abp.SysMng.FunctionPage;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Identity;
using Volo.Abp.TenantManagement;

namespace Nels.Abp.SysMng.EntityFrameworkCore;

[ConnectionStringName(SysMngDbProperties.ConnectionStringName)]
public class SysMngDbContext(DbContextOptions<SysMngDbContext> options) : AbpDbContext<SysMngDbContext>(options), ISysMngDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * public DbSet<Question> Questions { get; set; }
     */

    public DbSet<App> Apps { get; set; }
    public DbSet<BusinessUnit> BusinessUnits { get; set; }
    public DbSet<Page> Pages { get; set; }

    public DbSet<BizParameter> BizParameters { get; set; }
    public DbSet<BizParameterValue> BizParameterValues { get; set; }

    public DbSet<IdentityUser> Users { get; set; }

    public DbSet<IdentityRole> Roles { get; set; }

    public DbSet<IdentityUserRole> UserRoles { get; set; }

    public DbSet<IdentityClaimType> ClaimTypes { get; set; }

    public DbSet<OrganizationUnit> OrganizationUnits { get; set; }

    public DbSet<IdentitySecurityLog> SecurityLogs { get; set; }

    public DbSet<IdentityLinkUser> LinkUsers { get; set; }

    public DbSet<IdentityUserDelegation> UserDelegations { get; set; }

    public DbSet<Tenant> Tenants { get; set; }

    public DbSet<TenantConnectionString> TenantConnectionStrings { get; set; }

    public DbSet<IdentitySession> Sessions { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ConfigureSysMng();
    }
}
