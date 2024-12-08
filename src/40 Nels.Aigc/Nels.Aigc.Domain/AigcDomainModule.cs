using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Nels.Abp.SysMng;
using Nels.Abp.SysMng.FunctionPage;
using Nels.Aigc.MultiTenancy;
using Nels.Aigc.Providers;
using System;
using System.Collections.Generic;
using Volo.Abp.AuditLogging;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Data;
using Volo.Abp.Emailing;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.OpenIddict;
using Volo.Abp.SettingManagement;
using Volo.Abp.TenantManagement;

namespace Nels.Aigc;

[DependsOn(
    typeof(AbpAuditLoggingDomainModule),
    typeof(AbpBackgroundJobsDomainModule),
    typeof(AbpFeatureManagementDomainModule),
    typeof(AbpIdentityDomainModule),
    typeof(AbpOpenIddictDomainModule),
    typeof(AbpSettingManagementDomainModule),
    typeof(AbpTenantManagementDomainModule),
    typeof(AbpEmailingModule),
    typeof(NelsAbpSysMngDomainModule),
    typeof(AigcDomainSharedModule)
)]
public class AigcDomainModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        AutoAddDefinitionProviders(context.Services);
    }
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            //options.Languages.Add(new LanguageInfo("ar", "ar", "العربية", "ae"));
            //options.Languages.Add(new LanguageInfo("cs", "cs", "Čeština"));
            options.Languages.Add(new LanguageInfo("en", "en", "English"));
            //options.Languages.Add(new LanguageInfo("en-GB", "en-GB", "English (UK)"));
            //options.Languages.Add(new LanguageInfo("hu", "hu", "Magyar"));
            //options.Languages.Add(new LanguageInfo("hr", "hr", "Croatian"));
            //options.Languages.Add(new LanguageInfo("fi", "fi", "Finnish", "fi"));
            //options.Languages.Add(new LanguageInfo("fr", "fr", "Français", "fr"));
            //options.Languages.Add(new LanguageInfo("hi", "hi", "Hindi", "in"));
            //options.Languages.Add(new LanguageInfo("it", "it", "Italiano", "it"));
            //options.Languages.Add(new LanguageInfo("pt-BR", "pt-BR", "Português"));
            //options.Languages.Add(new LanguageInfo("ru", "ru", "Русский", "ru"));
            //options.Languages.Add(new LanguageInfo("sk", "sk", "Slovak", "sk"));
            //options.Languages.Add(new LanguageInfo("tr", "tr", "Türkçe", "tr"));
            options.Languages.Add(new LanguageInfo("zh-Hans", "zh-Hans", "简体中文"));
            options.Languages.Add(new LanguageInfo("zh-Hant", "zh-Hant", "繁體中文"));
            //options.Languages.Add(new LanguageInfo("de-DE", "de-DE", "Deutsch", "de"));
            //options.Languages.Add(new LanguageInfo("es", "es", "Español"));
        });

        Configure<AbpMultiTenancyOptions>(options =>
        {
            options.IsEnabled = MultiTenancyConsts.IsEnabled;
        });

#if DEBUG
        context.Services.Replace(ServiceDescriptor.Singleton<IEmailSender, NullEmailSender>());
#endif

    }

    #region functionPage
    private static void AutoAddDefinitionProviders(IServiceCollection services)
    {
        var definitionProviders = new List<Type>();
        var modelDefinitionProviders = new List<Type>();
        var promptDefinitionProviders = new List<Type>();

        services.OnRegistered(context =>
        {
            if (typeof(IFunctionPageProvider).IsAssignableFrom(context.ImplementationType))
            {
                definitionProviders.Add(context.ImplementationType);
            }
            else if (typeof(IModelProvider).IsAssignableFrom(context.ImplementationType))
            {
                modelDefinitionProviders.Add(context.ImplementationType);
            }
            else if (typeof(IPromptProvider).IsAssignableFrom(context.ImplementationType))
            {
                promptDefinitionProviders.Add(context.ImplementationType);
            }
        });

        services.Configure<FunctionPageOptions>(options =>
        {
            options.DefinitionProviders.AddIfNotContains(definitionProviders);
        });
        services.Configure<ModelOptions>(options =>
        {
            options.DefinitionProviders.AddIfNotContains(modelDefinitionProviders);
        });
        services.Configure<PromptOptions>(options =>
        {
            options.DefinitionProviders.AddIfNotContains(promptDefinitionProviders);
        });
    }
    #endregion
}