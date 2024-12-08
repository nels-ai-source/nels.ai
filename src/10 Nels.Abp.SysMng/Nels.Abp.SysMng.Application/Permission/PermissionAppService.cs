using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;
using Volo.Abp.PermissionManagement;
using Volo.Abp.SimpleStateChecking;

namespace Nels.Abp.SysMng.Permission;

[Route(SysMngRemoteServiceConsts.PermissionRoute)]
public class PermissionAppService(
    IPermissionManager permissionManager,
    IPermissionDefinitionManager permissionDefinitionManager,
    IOptions<PermissionManagementOptions> options,
    ISimpleStateCheckerManager<PermissionDefinition> simpleStateCheckerManager) : ApplicationService
{
    protected PermissionManagementOptions Options { get; } = options.Value;
    protected IPermissionManager PermissionManager { get; } = permissionManager;
    protected IPermissionDefinitionManager PermissionDefinitionManager { get; } = permissionDefinitionManager;
    protected ISimpleStateCheckerManager<PermissionDefinition> SimpleStateCheckerManager { get; } = simpleStateCheckerManager;

    [HttpPost]
    [Route(Ddd.Application.Contracts.RemoteServiceConsts.ApiRouteGet)]
    public virtual async Task<List<PermissionGroupDto>> GetAsync(string providerName, string providerKey)
    {
        await CheckProviderPolicy(providerName);

        var result = new List<PermissionGroupDto>();

        var multiTenancySide = CurrentTenant.GetMultiTenancySide();
        var ignoreGroup = new List<string> { "AbpIdentity", "FeatureManagement", "SettingManagement", "AbpTenantManagement" };
        var groups = (await PermissionDefinitionManager.GetGroupsAsync())
            .Where(x => !ignoreGroup.Contains(x.Name)).ToList();


        foreach (var group in groups)
        {
            var groupDto = CreatePermissionGroupDto(group);

            var neededCheckPermissions = new List<PermissionDefinition>();

            foreach (var permission in group.GetPermissionsWithChildren()
                                            .Where(x => x.IsEnabled)
                                            .Where(x => !x.Providers.Any() || x.Providers.Contains(providerName))
                                            .Where(x => x.MultiTenancySide.HasFlag(multiTenancySide)))
            {
                if (await SimpleStateCheckerManager.IsEnabledAsync(permission))
                {
                    neededCheckPermissions.Add(permission);
                }
            }

            if (!neededCheckPermissions.Any())
            {
                continue;
            }

            var grantInfoDtos = neededCheckPermissions
                .Where(x => x.Parent == null)
                .Select(CreatePermissionGrantInfoDto)
                .ToList();

            var multipleGrantInfo = await PermissionManager.GetAsync(neededCheckPermissions.Select(x => x.Name).ToArray(), providerName, providerKey);
            foreach (var grantInfoDto in grantInfoDtos)
            {
                grantInfoDto.IsGranted = multipleGrantInfo.Result.First(x => x.Name == grantInfoDto.Name).IsGranted;
                if (grantInfoDto.Children != null)
                {
                    foreach (var item in grantInfoDto.Children)
                    {
                        item.IsGranted = multipleGrantInfo.Result.First(x => x.Name == item.Name).IsGranted;
                    }
                }
                groupDto.Permissions.Add(grantInfoDto);
            }

            if (groupDto.Permissions.Any())
            {
                result.Add(groupDto);
            }
        }

        return result;
    }

    private PermissionGrantDto CreatePermissionGrantInfoDto(PermissionDefinition permission)
    {
        var permissionDto = new PermissionGrantDto();

        permissionDto.Name = permission.Name;
        permissionDto.DisplayName = permission.DisplayName.Localize(StringLocalizerFactory);
        permissionDto.ParentName = permission.Parent?.Name;
        if (permission.Children.Any())
        {
            permissionDto.Children = permission.Children.Select(CreatePermissionGrantInfoDto).ToList();
        }
        return permissionDto;

    }

    private PermissionGroupDto CreatePermissionGroupDto(PermissionGroupDefinition group)
    {
        var localizableDisplayName = group.DisplayName as LocalizableString;

        return new PermissionGroupDto
        {
            Name = group.Name,
            DisplayName = group.DisplayName.Localize(StringLocalizerFactory),
            Permissions = new List<PermissionGrantDto>()
        };
    }

    [HttpPost]
    [Route(Ddd.Application.Contracts.RemoteServiceConsts.ApiRouteUpdate)]
    public virtual async Task UpdateAsync(string providerName, string providerKey, UpdatePermissionsDto input)
    {
        await CheckProviderPolicy(providerName);

        foreach (var permissionDto in input.Permissions)
        {
            await PermissionManager.SetAsync(permissionDto.Name, providerName, providerKey, permissionDto.IsGranted);
        }
    }

    protected virtual async Task CheckProviderPolicy(string providerName)
    {
        var policyName = Options.ProviderPolicies.GetOrDefault(providerName);
        if (policyName.IsNullOrEmpty())
        {
            throw new AbpException($"No policy defined to get/set permissions for the provider '{providerName}'. Use {nameof(PermissionManagementOptions)} to map the policy.");
        }

        await AuthorizationService.CheckAsync(policyName);
    }
}
