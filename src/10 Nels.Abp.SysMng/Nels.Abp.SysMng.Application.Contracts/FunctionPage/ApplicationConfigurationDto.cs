using Nels.Abp.SysMng.FunctionPage.MultiTenancy;
using Nels.Abp.SysMng.FunctionPage.ObjectExtending;
using System;
using Volo.Abp.Data;

namespace Nels.Abp.SysMng.FunctionPage;

[Serializable]
public class ApplicationConfigurationDto : IHasExtraProperties
{
    public ApplicationLocalizationConfigurationDto Localization { get; set; }

    public ApplicationAuthConfigurationDto Auth { get; set; }

    public ApplicationSettingConfigurationDto Setting { get; set; }

    public CurrentUserDto CurrentUser { get; set; }

    public ApplicationFeatureConfigurationDto Features { get; set; }

    public ApplicationGlobalFeatureConfigurationDto GlobalFeatures { get; set; }

    public MultiTenancyInfoDto MultiTenancy { get; set; }

    public CurrentTenantDto CurrentTenant { get; set; }

    public TimingDto Timing { get; set; }

    public ClockDto Clock { get; set; }

    public ObjectExtensionsDto ObjectExtensions { get; set; }

    public ExtraPropertyDictionary ExtraProperties { get; set; }
}
