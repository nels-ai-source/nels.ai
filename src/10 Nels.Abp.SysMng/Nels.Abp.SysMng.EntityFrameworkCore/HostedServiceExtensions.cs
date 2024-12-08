using Volo.Abp.AuditLogging;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.SettingManagement;
using Volo.Abp.TenantManagement;

namespace Nels.Abp.SysMng;

public static class HostedServiceExtensions
{
    private static readonly string sysDbTablePrefix = "sys_";
    public static void SetDbTablePrefix()
    {
        //AbpIdentityDbProperties.DbTablePrefix = sysDbTablePrefix;
        //AbpAuditLoggingDbProperties.DbTablePrefix = sysDbTablePrefix;
        //AbpBackgroundJobsDbProperties.DbTablePrefix = sysDbTablePrefix;
        //AbpPermissionManagementDbProperties.DbTablePrefix = sysDbTablePrefix;
        //AbpFeatureManagementDbProperties.DbTablePrefix = sysDbTablePrefix;
        //AbpSettingManagementDbProperties.DbTablePrefix = sysDbTablePrefix;
        //AbpTenantManagementDbProperties.DbTablePrefix = sysDbTablePrefix;
    }
}
