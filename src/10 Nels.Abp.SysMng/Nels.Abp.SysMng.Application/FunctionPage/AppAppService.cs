using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Nels.Abp.Ddd.Application.Contracts;
using Nels.Abp.Ddd.Application.Services;
using Nels.Abp.SysMng.FunctionPage.MultiTenancy;
using Nels.Abp.SysMng.Localization;
using Nels.Abp.SysMng.Permissions;
using Nels.Abp.SysMng.Settings;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations;
using Volo.Abp.Authorization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Features;
using Volo.Abp.GlobalFeatures;
using Volo.Abp.Localization;
using Volo.Abp.Localization.External;
using Volo.Abp.MultiTenancy;
using Volo.Abp.SettingManagement;
using Volo.Abp.Settings;
using Volo.Abp.Timing;
using Volo.Abp.Users;

namespace Nels.Abp.SysMng.FunctionPage;


[Route(SysMngRemoteServiceConsts.appRoute)]
public class AppAppService : RouteCrudGetAllAppService<App, AppDto, Guid>
{
    private readonly ICurrentUser _currentUser;
    private readonly ISettingManager _settingManager;
    private readonly AppDomainService _appDomainService;
    private readonly IRepository<App, Guid> _repository;
    private readonly AbpLocalizationOptions _localizationOptions;
    private readonly AbpMultiTenancyOptions _multiTenancyOptions;
    private readonly IAbpAuthorizationPolicyProvider _abpAuthorizationPolicyProvider;
    private readonly IPermissionDefinitionManager _permissionDefinitionManager;
    private readonly DefaultAuthorizationPolicyProvider _defaultAuthorizationPolicyProvider;
    private readonly IPermissionChecker _permissionChecker;
    private readonly IAuthorizationService _authorizationService;
    private readonly ISettingProvider _settingProvider;
    private readonly ISettingDefinitionManager _settingDefinitionManager;
    private readonly IFeatureDefinitionManager _featureDefinitionManager;
    private readonly ILanguageProvider _languageProvider;
    private readonly ITimezoneProvider _timezoneProvider;
    private readonly AbpClockOptions _abpClockOptions;
    public AppAppService(IRepository<App, Guid> repository, ICurrentUser currentUser, AppDomainService appDomainService, ISettingManager settingManager,
               IOptions<AbpLocalizationOptions> localizationOptions,
        IOptions<AbpMultiTenancyOptions> multiTenancyOptions,
        IServiceProvider serviceProvider,
        IAbpAuthorizationPolicyProvider abpAuthorizationPolicyProvider,
        IPermissionDefinitionManager permissionDefinitionManager,
        DefaultAuthorizationPolicyProvider defaultAuthorizationPolicyProvider,
        IPermissionChecker permissionChecker,
        IAuthorizationService authorizationService,
        ISettingProvider settingProvider,
        ISettingDefinitionManager settingDefinitionManager,
        IFeatureDefinitionManager featureDefinitionManager,
        ILanguageProvider languageProvider,
        ITimezoneProvider timezoneProvider,
        IOptions<AbpClockOptions> abpClockOptions) : base(repository)
    {
        CreatePolicyName = SysMngPermissions.App.Create;
        UpdatePolicyName = SysMngPermissions.App.Update;
        DeletePolicyName = SysMngPermissions.App.Delete;
        GetPolicyName = SysMngPermissions.App.GetList;
        GetListPolicyName = SysMngPermissions.App.GetList;

        _currentUser = currentUser;
        _settingManager = settingManager;
        _appDomainService = appDomainService;
        _repository = repository;

        _abpAuthorizationPolicyProvider = abpAuthorizationPolicyProvider;
        _permissionDefinitionManager = permissionDefinitionManager;
        _defaultAuthorizationPolicyProvider = defaultAuthorizationPolicyProvider;
        _permissionChecker = permissionChecker;
        _authorizationService = authorizationService;
        _currentUser = currentUser;
        _settingProvider = settingProvider;
        _settingDefinitionManager = settingDefinitionManager;
        _featureDefinitionManager = featureDefinitionManager;
        _languageProvider = languageProvider;
        _timezoneProvider = timezoneProvider;
        _abpClockOptions = abpClockOptions.Value;
        _localizationOptions = localizationOptions.Value;
        _multiTenancyOptions = multiTenancyOptions.Value;

        LocalizationResource = typeof(MenuResource);
    }

    [HttpPost]
    [Route(RemoteServiceConsts.ApiRouteGetConfiguration)]
    public async Task<UserMenuOutputDto> GetConfigurationAsync(Guid? applicationId = null)
    {
        if (!_currentUser.IsAuthenticated)
        {
            throw new AbpAuthorizationException();
        }

        if (applicationId == null || applicationId == Guid.Empty)
        {
            var currentAppId = await _settingManager.GetOrNullForCurrentUserAsync(SysMngSettingConst.CurrentAppIdName, false);
            if (string.IsNullOrWhiteSpace(currentAppId) == false)
            {
                applicationId = Guid.Parse(currentAppId);
            }
        }
        else
        {
            await _settingManager.SetForCurrentUserAsync(SysMngSettingConst.CurrentAppIdName, applicationId.ToString());
        }

        var permissions = await _appDomainService.GetUserPermissionGrantsAsync(_currentUser.Id ?? Guid.Empty);
        var menus = await GetAppMenusAsync(applicationId);

        var userMenuOutput = new UserMenuOutputDto
        {
            Menus = menus,
            UserInfo = _currentUser,
            Permissions = permissions,
            Localization = await GetLocalizationConfigAsync(),
        };
        return await Task.FromResult(userMenuOutput);
    }

    private async Task<List<MenuDto>> GetAppMenusAsync(Guid? applicationId)
    {
        var apps = MapList<App, AppDto>((await _repository.GetListAsync(x => x.IsEnable)));
        var currentAppId = applicationId == null || applicationId == Guid.Empty ? apps.First().Id : applicationId.Value;
        var currentApp = apps.First(x => x.Id == currentAppId);

        currentApp.DisplayName = GetDisplayName(currentApp.DisplayName);

        var permissions = await _appDomainService.GetUserPermissionGrantsAsync(_currentUser.Id ?? Guid.Empty);
        var (businesses, pages) = await _appDomainService.GetAppMeunsAsync(Guid.Parse(currentAppId.ToString()), permissions);
        businesses.ForEach(x => x.DisplayName = GetDisplayName(x.DisplayName));
        pages.ForEach(x => x.DisplayName = GetDisplayName(x.DisplayName));

        var menus = MapList<BusinessUnit, MenuDto>(businesses);
        foreach (var menu in menus)
        {
            menu.Children = MapList<Page, MenuDto>(pages.Where(x => x.BusinessUnitId == menu.Id).ToList());
        }

        //var appEnum = new MenuDto
        //{
        //    Id = Guid.Empty,
        //    Name = "应用",
        //    Path = Guid.Empty.ToString(),
        //    Meta = new MenuMetaDto
        //    {
        //        Title = "应用",
        //        Icon = "el-icon-menu"
        //    },
        //    Children = apps.Where(x => x.Id != currentApp.Id).Select(x => new MenuDto
        //    {
        //        Id = x.Id,
        //        Name = x.Name,
        //        ParentId = Guid.Empty,
        //        Meta = new MenuMetaDto
        //        {
        //            Title = x.DisplayName,
        //            Icon = x.Icon
        //        },
        //    }).ToList()
        //};

        //menus.Add(appEnum);
        menus.Insert(0, (new MenuDto
        {
            Id = Guid.NewGuid(),
            Name = "home",
            Path = "/home",
            Meta = new MenuMetaDto
            {
                Title = GetDisplayName("Menu:Home"),
                Icon = "el-icon-eleme-filled",
            },
            Children =
            [
                new MenuDto
                {
                    Name= "dashboard",
                    Path= "/dashboard",
                    Component ="home",
                    Meta=new MenuMetaDto
                    {
                        Title= GetDisplayName("Menu:Dashboard"),
                        Icon= "el-icon-menu",
                        Affix=true
                    }
                }
            ]
        }));

        return await Task.FromResult(menus);
    }

    [HttpGet]
    public virtual async Task<ApplicationConfigurationDto> GetAsync()
    {
        var result = new ApplicationConfigurationDto
        {
            Auth = await GetAuthConfigAsync(),
            Features = await GetFeaturesConfigAsync(),
            GlobalFeatures = await GetGlobalFeaturesConfigAsync(),
            Localization = await GetLocalizationConfigAsync(),
            CurrentUser = GetCurrentUser(),
            Setting = await GetSettingConfigAsync(),
            MultiTenancy = GetMultiTenancy(),
            CurrentTenant = GetCurrentTenant(),
            Timing = await GetTimingConfigAsync(),
            Clock = GetClockConfig()
        };


        return result;
    }

    protected virtual CurrentTenantDto GetCurrentTenant()
    {
        return new CurrentTenantDto()
        {
            Id = CurrentTenant.Id,
            Name = CurrentTenant.Name,
            IsAvailable = CurrentTenant.IsAvailable
        };
    }

    protected virtual MultiTenancyInfoDto GetMultiTenancy()
    {
        return new MultiTenancyInfoDto
        {
            IsEnabled = _multiTenancyOptions.IsEnabled
        };
    }

    protected virtual CurrentUserDto GetCurrentUser()
    {
        return new CurrentUserDto
        {
            IsAuthenticated = _currentUser.IsAuthenticated,
            Id = _currentUser.Id,
            TenantId = _currentUser.TenantId,
            ImpersonatorUserId = _currentUser.FindImpersonatorUserId(),
            ImpersonatorTenantId = _currentUser.FindImpersonatorTenantId(),
            ImpersonatorUserName = _currentUser.FindImpersonatorUserName(),
            ImpersonatorTenantName = _currentUser.FindImpersonatorTenantName(),
            UserName = _currentUser.UserName,
            SurName = _currentUser.SurName,
            Name = _currentUser.Name,
            Email = _currentUser.Email,
            EmailVerified = _currentUser.EmailVerified,
            PhoneNumber = _currentUser.PhoneNumber,
            PhoneNumberVerified = _currentUser.PhoneNumberVerified,
            Roles = _currentUser.Roles
        };
    }

    protected virtual async Task<ApplicationAuthConfigurationDto> GetAuthConfigAsync()
    {
        var authConfig = new ApplicationAuthConfigurationDto();

        var policyNames = await _abpAuthorizationPolicyProvider.GetPoliciesNamesAsync();
        var abpPolicyNames = new List<string>();
        var otherPolicyNames = new List<string>();

        foreach (var policyName in policyNames)
        {
            if (await _defaultAuthorizationPolicyProvider.GetPolicyAsync(policyName) == null &&
                await _permissionDefinitionManager.GetOrNullAsync(policyName) != null)
            {
                abpPolicyNames.Add(policyName);
            }
            else
            {
                otherPolicyNames.Add(policyName);
            }
        }

        foreach (var policyName in otherPolicyNames)
        {
            if (await _authorizationService.IsGrantedAsync(policyName))
            {
                authConfig.GrantedPolicies[policyName] = true;
            }
        }

        var result = await _permissionChecker.IsGrantedAsync(abpPolicyNames.ToArray());
        foreach (var (key, value) in result.Result)
        {
            if (value == PermissionGrantResult.Granted)
            {
                authConfig.GrantedPolicies[key] = true;
            }
        }

        return authConfig;
    }

    protected virtual async Task<ApplicationLocalizationConfigurationDto> GetLocalizationConfigAsync()
    {
        var localizationConfig = new ApplicationLocalizationConfigurationDto();

        localizationConfig.Languages.AddRange(await _languageProvider.GetLanguagesAsync());
        var options = new ApplicationConfigurationRequestOptions
        {
            IncludeLocalizationResources = true
        };

        if (options.IncludeLocalizationResources)
        {
            var resourceNames = _localizationOptions
                .Resources
                .Values
                .Select(x => x.ResourceName)
                .Union(
                    await LazyServiceProvider
                        .LazyGetRequiredService<IExternalLocalizationStore>()
                        .GetResourceNamesAsync()
                );

            foreach (var resourceName in resourceNames)
            {
                var dictionary = new Dictionary<string, string>();

                var localizer = await StringLocalizerFactory
                    .CreateByResourceNameOrNullAsync(resourceName);

                if (localizer != null)
                {
                    foreach (var localizedString in await localizer.GetAllStringsAsync())
                    {
                        dictionary[localizedString.Name] = localizedString.Value;
                    }
                }

                localizationConfig.Values[resourceName] = dictionary;
            }
        }

        localizationConfig.CurrentCulture = GetCurrentCultureInfo();

        if (_localizationOptions.DefaultResourceType != null)
        {
            localizationConfig.DefaultResourceName = LocalizationResourceNameAttribute.GetName(
                _localizationOptions.DefaultResourceType
            );
        }

        localizationConfig.LanguagesMap = _localizationOptions.LanguagesMap;
        localizationConfig.LanguageFilesMap = _localizationOptions.LanguageFilesMap;

        return localizationConfig;
    }

    private static CurrentCultureDto GetCurrentCultureInfo()
    {
        return new CurrentCultureDto
        {
            Name = CultureInfo.CurrentUICulture.Name,
            DisplayName = CultureInfo.CurrentUICulture.DisplayName,
            EnglishName = CultureInfo.CurrentUICulture.EnglishName,
            NativeName = CultureInfo.CurrentUICulture.NativeName,
            IsRightToLeft = CultureInfo.CurrentUICulture.TextInfo.IsRightToLeft,
            CultureName = CultureInfo.CurrentUICulture.TextInfo.CultureName,
            TwoLetterIsoLanguageName = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName,
            ThreeLetterIsoLanguageName = CultureInfo.CurrentUICulture.ThreeLetterISOLanguageName,
            DateTimeFormat = new DateTimeFormatDto
            {
                CalendarAlgorithmType =
                    CultureInfo.CurrentUICulture.DateTimeFormat.Calendar.AlgorithmType.ToString(),
                DateTimeFormatLong = CultureInfo.CurrentUICulture.DateTimeFormat.LongDatePattern,
                ShortDatePattern = CultureInfo.CurrentUICulture.DateTimeFormat.ShortDatePattern,
                FullDateTimePattern = CultureInfo.CurrentUICulture.DateTimeFormat.FullDateTimePattern,
                DateSeparator = CultureInfo.CurrentUICulture.DateTimeFormat.DateSeparator,
                ShortTimePattern = CultureInfo.CurrentUICulture.DateTimeFormat.ShortTimePattern,
                LongTimePattern = CultureInfo.CurrentUICulture.DateTimeFormat.LongTimePattern,
            }
        };
    }

    private async Task<ApplicationSettingConfigurationDto> GetSettingConfigAsync()
    {
        var result = new ApplicationSettingConfigurationDto
        {
            Values = new Dictionary<string, string>()
        };

        var settingDefinitions = (await _settingDefinitionManager.GetAllAsync()).Where(x => x.IsVisibleToClients);

        var settingValues = await _settingProvider.GetAllAsync(settingDefinitions.Select(x => x.Name).ToArray());

        foreach (var settingValue in settingValues)
        {
            result.Values[settingValue.Name] = settingValue.Value;
        }

        return result;
    }

    protected virtual async Task<ApplicationFeatureConfigurationDto> GetFeaturesConfigAsync()
    {
        var result = new ApplicationFeatureConfigurationDto();

        foreach (var featureDefinition in await _featureDefinitionManager.GetAllAsync())
        {
            if (!featureDefinition.IsVisibleToClients)
            {
                continue;
            }

            result.Values[featureDefinition.Name] = await FeatureChecker.GetOrNullAsync(featureDefinition.Name);
        }

        return result;
    }

    protected virtual Task<ApplicationGlobalFeatureConfigurationDto> GetGlobalFeaturesConfigAsync()
    {
        var result = new ApplicationGlobalFeatureConfigurationDto();

        foreach (var enabledFeatureName in GlobalFeatureManager.Instance.GetEnabledFeatureNames())
        {
            result.EnabledFeatures.AddIfNotContains(enabledFeatureName);
        }

        return Task.FromResult(result);
    }

    protected virtual async Task<TimingDto> GetTimingConfigAsync()
    {
        var windowsTimeZoneId = await _settingProvider.GetOrNullAsync(TimingSettingNames.TimeZone);

        return new TimingDto
        {
            TimeZone = new TimeZone
            {
                Windows = new WindowsTimeZone
                {
                    TimeZoneId = windowsTimeZoneId
                },
                Iana = new IanaTimeZone
                {
                    TimeZoneName = windowsTimeZoneId.IsNullOrWhiteSpace()
                        ? null
                        : _timezoneProvider.WindowsToIana(windowsTimeZoneId)
                }
            }
        };
    }

    protected virtual ClockDto GetClockConfig()
    {
        return new ClockDto
        {
            Kind = Enum.GetName(typeof(DateTimeKind), _abpClockOptions.Kind)
        };
    }
}
