using Microsoft.EntityFrameworkCore;
using Nels.Abp.SysMng.EntityFrameworkCore;
using Nels.Abp.SysMng.FunctionPage;
using Nels.Aigc.Entities;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Identity;
using Volo.Abp.TenantManagement;

namespace Nels.Aigc.EntityFrameworkCore;

[ReplaceDbContext(typeof(ISysMngDbContext))]
[ConnectionStringName(AigcDbProperties.ConnectionStringName)]
public class AigcDbContext(DbContextOptions<AigcDbContext> options) : AbpDbContext<AigcDbContext>(options), IAigcDbContext
{
    /* Add DbSet properties for your Aggregate Roots / Entities here. */

    #region Entities from the modules

    /* Notice: We only implemented IIdentityDbContext and ITenantManagementDbContext
     * and replaced them for this DbContext. This allows you to perform JOIN
     * queries for the entities of these modules over the repositories easily. You
     * typically don't need that for other modules. But, if you need, you can
     * implement the DbContext interface of the needed module and use ReplaceDbContext
     * attribute just like IIdentityDbContext and ITenantManagementDbContext.
     *
     * More info: Replacing a DbContext of a module ensures that the related module
     * uses this DbContext on runtime. Otherwise, it will use its own DbContext class.
     */

    //Identity
    public DbSet<IdentityUser> Users { get; set; }
    public DbSet<IdentityRole> Roles { get; set; }
    public DbSet<IdentityClaimType> ClaimTypes { get; set; }
    public DbSet<OrganizationUnit> OrganizationUnits { get; set; }
    public DbSet<IdentitySecurityLog> SecurityLogs { get; set; }
    public DbSet<IdentityLinkUser> LinkUsers { get; set; }
    public DbSet<IdentityUserDelegation> UserDelegations { get; set; }

    // Tenant Management
    public DbSet<Tenant> Tenants { get; set; }
    public DbSet<TenantConnectionString> TenantConnectionStrings { get; set; }
    public DbSet<App> Apps { get; set; }
    public DbSet<BusinessUnit> BusinessUnits { get; set; }
    public DbSet<Page> Pages { get; set; }
    public DbSet<BizParameter> BizParameters { get; set; }
    public DbSet<BizParameterValue> BizParameterValues { get; set; }

    public DbSet<IdentitySession> Sessions { get; set; }

    #endregion
    public DbSet<Model> Models { get; set; }
    public DbSet<ModelInstance> ModelInstances { get; set; }
    public DbSet<Prompt> Prompts { get; set; }
    public DbSet<Agent> Agents { get; set; }
    public DbSet<AgentPresetQuestions> AgentPresetQuestions { get; set; }
    public DbSet<AgentMetadata> AgentMetadatas { get; set; }

    public DbSet<Space> Spaces { get; set; }
    public DbSet<SpaceUser> SpaceUsers { get; set; }

    public DbSet<Knowledge> Knowledges { get; set; }
    public DbSet<KnowledgeDocument> KnowledgeDocuments { get; set; }
    public DbSet<KnowledgeDocumentParagraph> KnowledgeDocumentParagraphs { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        /* Include modules to your migration db context */
        builder.ConfigureSysMng();
        builder.ConfigureAigc();
        /* Configure your own tables/entities inside here */

        //builder.Entity<YourEntity>(b =>
        //{
        //    b.ToTable(AgentConsts.DbTablePrefix + "YourEntities", AgentConsts.DbSchema);
        //    b.ConfigureByConvention(); //auto configure for the base class props
        //    //...
        //});
    }
}
