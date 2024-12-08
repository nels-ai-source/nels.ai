using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Nels.Abp.Ddd.Application.Services;
using Nels.Abp.SysMng.Permissions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Data;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;
using Volo.Abp.ObjectExtending;
using Volo.Abp.PermissionManagement;
using IdentityRole = Volo.Abp.Identity.IdentityRole;

namespace Nels.Abp.SysMng.Identity;

/// <summary>
/// Role service class that handles role management operations including creation,updating, deletion and querying of roles, as well as role permission management.
/// </summary>
[Route(SysMngRemoteServiceConsts.RoleRoute)]
public class RoleAppService : RouteCrudAppService<Volo.Abp.Identity.IdentityRole, IdentityRoleDto, Guid, GetIdentityRolesInput, IdentityRoleCreateDto, IdentityRoleUpdateDto>
{
    public IdentityRoleManager RoleManager { get; }
    public IIdentityRoleRepository RoleRepository { get; }
    public IStaticPermissionDefinitionStore StaticPermissionDefinitionStore { get; }
    public IPermissionAppService PermissionAppService { get; }

    public RoleAppService(IRepository<Volo.Abp.Identity.IdentityRole, Guid> repository, IdentityRoleManager roleManager,
        IIdentityRoleRepository roleRepository,
        IStaticPermissionDefinitionStore staticPermissionDefinitionStore,
        IPermissionAppService permissionAppService
        ) : base(repository)
    {
        RoleManager = roleManager;
        RoleRepository = roleRepository;
        StaticPermissionDefinitionStore = staticPermissionDefinitionStore;
        PermissionAppService = permissionAppService;

        CreatePolicyName = SysMngPermissions.Role.Create;
        UpdatePolicyName = SysMngPermissions.Role.Update;
        DeletePolicyName = SysMngPermissions.Role.Delete;
        GetPolicyName = SysMngPermissions.Role.GetList;
        GetListPolicyName = SysMngPermissions.Role.GetList;
    }
    [HttpPost]
    [Route(Ddd.Application.Contracts.RemoteServiceConsts.ApiRouteCreate)]
    public override async Task<IdentityRoleDto> CreateAsync(IdentityRoleCreateDto input)
    {
        await CheckCreatePolicyAsync();

        var role = new IdentityRole(
            GuidGenerator.Create(),
            input.Name,
            CurrentTenant.Id
        )
        {
            IsDefault = input.IsDefault,
            IsPublic = input.IsPublic
        };

        input.MapExtraPropertiesTo(role);

        (await RoleManager.CreateAsync(role)).CheckErrors();
        await CurrentUnitOfWork.SaveChangesAsync();

        return ObjectMapper.Map<IdentityRole, IdentityRoleDto>(role);
    }
    [HttpPost]
    [Route(Ddd.Application.Contracts.RemoteServiceConsts.ApiRouteUpdate)]
    public override async Task<IdentityRoleDto> UpdateAsync(Guid id, IdentityRoleUpdateDto input)
    {
        await CheckUpdatePolicyAsync();

        var role = await RoleManager.GetByIdAsync(id);

        role.SetConcurrencyStampIfNotNull(input.ConcurrencyStamp);

        (await RoleManager.SetRoleNameAsync(role, input.Name)).CheckErrors();

        role.IsDefault = input.IsDefault;
        role.IsPublic = input.IsPublic;

        input.MapExtraPropertiesTo(role);

        (await RoleManager.UpdateAsync(role)).CheckErrors();
        await CurrentUnitOfWork.SaveChangesAsync();

        return ObjectMapper.Map<IdentityRole, IdentityRoleDto>(role);
    }
    [HttpPost]
    [Route(Ddd.Application.Contracts.RemoteServiceConsts.ApiRouteDelete)]
    public override async Task DeleteAsync(Guid id)
    {
        await CheckDeletePolicyAsync();

        var role = await RoleManager.FindByIdAsync(id.ToString());
        if (role == null)
        {
            return;
        }

        (await RoleManager.DeleteAsync(role)).CheckErrors();
    }
    [HttpPost]
    [Route(Ddd.Application.Contracts.RemoteServiceConsts.ApiRouteGetRolePermissions)]
    public virtual async Task<List<PermissionGroupDto>> GetRolePermissions(Guid id)
    {
        await CheckPolicyAsync(SysMngPermissions.Role.ManagePermissions);
        var result = new List<PermissionGroupDto>();
        var role = await RoleManager.FindByIdAsync(id.ToString());
        if (role == null)
        {
            return result;
        }
        var permissionListResultDto = await PermissionAppService.GetAsync("R", role.Name);

        foreach (var permissionGroup in permissionListResultDto.Groups)
        {
            var dto = new PermissionGroupDto();
            dto.Name = permissionGroup.Name;
            dto.DisplayName = permissionGroup.DisplayName;
            dto.Permissions = permissionGroup.Permissions;

            result.Add(permissionGroup);
        }

        return result;
    }
}
