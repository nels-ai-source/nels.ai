using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Nels.Abp.Ddd.Application.Services;
using Nels.Abp.SysMng.Permissions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;
using Volo.Abp.ObjectExtending;
using IdentityUser = Volo.Abp.Identity.IdentityUser;

namespace Nels.Abp.SysMng.Identity;

/// <summary>
/// 用户服务类
/// </summary>
[Route(SysMngRemoteServiceConsts.UserRoute)]
public class UserAppService : RouteCrudAppService<IdentityUser, IdentityUserDto, Guid, GetIdentityUsersInput, IdentityUserCreateDto, IdentityUserUpdateDto>
{
    private IdentityUserManager UserManager { get; }
    private IOptions<IdentityOptions> IdentityOptions { get; }
    public UserAppService(IRepository<IdentityUser, Guid> repository,
        IdentityUserManager userManager,
        IOptions<IdentityOptions> identityOptions) : base(repository)
    {
        UserManager = userManager;
        IdentityOptions = identityOptions;

        CreatePolicyName = SysMngPermissions.User.Create;
        UpdatePolicyName = SysMngPermissions.User.Update;
        DeletePolicyName = SysMngPermissions.User.Delete;
        GetPolicyName = SysMngPermissions.User.GetList;
        GetListPolicyName = SysMngPermissions.User.GetList;
    }
    [HttpPost]
    [Route(Ddd.Application.Contracts.RemoteServiceConsts.ApiRouteCreate)]
    public override async Task<IdentityUserDto> CreateAsync(IdentityUserCreateDto input)
    {
        await CheckCreatePolicyAsync();

        await IdentityOptions.SetAsync();

        var user = new IdentityUser(
            GuidGenerator.Create(),
            input.UserName,
            input.Email,
            CurrentTenant.Id
        );

        input.MapExtraPropertiesTo(user);

        (await UserManager.CreateAsync(user, input.Password)).CheckErrors();
        await UpdateUserByInputAsync(user, input);
        (await UserManager.UpdateAsync(user)).CheckErrors();

        await CurrentUnitOfWork.SaveChangesAsync();

        return ObjectMapper.Map<IdentityUser, IdentityUserDto>(user);
    }
    [HttpPost]
    [Route(Ddd.Application.Contracts.RemoteServiceConsts.ApiRouteUpdate)]
    public override async Task<IdentityUserDto> UpdateAsync(Guid id, IdentityUserUpdateDto input)
    {
        await CheckUpdatePolicyAsync();

        await IdentityOptions.SetAsync();

        var user = await UserManager.GetByIdAsync(id);

        user.SetConcurrencyStampIfNotNull(input.ConcurrencyStamp);

        (await UserManager.SetUserNameAsync(user, input.UserName)).CheckErrors();

        await UpdateUserByInputAsync(user, input);
        input.MapExtraPropertiesTo(user);

        (await UserManager.UpdateAsync(user)).CheckErrors();

        if (!input.Password.IsNullOrEmpty())
        {
            (await UserManager.RemovePasswordAsync(user)).CheckErrors();
            (await UserManager.AddPasswordAsync(user, input.Password)).CheckErrors();
        }

        await CurrentUnitOfWork.SaveChangesAsync();

        return ObjectMapper.Map<IdentityUser, IdentityUserDto>(user);
    }
    [HttpPost]
    [Route(Ddd.Application.Contracts.RemoteServiceConsts.ApiRouteDelete)]
    public override async Task DeleteAsync(Guid id)
    {
        await CheckDeletePolicyAsync();

        if (CurrentUser.Id == id)
        {
            throw new BusinessException(code: IdentityErrorCodes.UserSelfDeletion);
        }

        var user = await UserManager.FindByIdAsync(id.ToString());
        if (user == null)
        {
            return;
        }

    (await UserManager.DeleteAsync(user)).CheckErrors();
    }
    protected virtual async Task UpdateUserByInputAsync(IdentityUser user, IdentityUserCreateOrUpdateDtoBase input)
    {
        if (!string.Equals(user.Email, input.Email, StringComparison.InvariantCultureIgnoreCase))
        {
            (await UserManager.SetEmailAsync(user, input.Email)).CheckErrors();
        }

        if (!string.Equals(user.PhoneNumber, input.PhoneNumber, StringComparison.InvariantCultureIgnoreCase))
        {
            (await UserManager.SetPhoneNumberAsync(user, input.PhoneNumber)).CheckErrors();
        }

       (await UserManager.SetLockoutEnabledAsync(user, input.LockoutEnabled)).CheckErrors();

        user.Name = input.Name;
        user.Surname = input.Surname;
        (await UserManager.UpdateAsync(user)).CheckErrors();
        user.SetIsActive(input.IsActive);
        if (input.RoleNames != null)
        {
            (await UserManager.SetRolesAsync(user, input.RoleNames)).CheckErrors();
        }
    }


}
