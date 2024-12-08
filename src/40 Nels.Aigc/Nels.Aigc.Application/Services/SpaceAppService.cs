using Microsoft.AspNetCore.Mvc;
using Nels.Abp.Ddd.Application.Services;
using Nels.Aigc.Dtos;
using Nels.Aigc.Entities;
using Nels.Aigc.Permissions;
using Nels.Aigc.Repositories;
using Nels.Aigc.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;
using Volo.Abp.SettingManagement;

namespace Nels.Aigc.Services;


[Route(AigcRemoteServiceConsts.spaceRoute)]
public class SpaceAppService : RouteCrudGetAllAppService<Space, SpaceDto, Guid>
{
    private readonly ISettingManager _settingManager;
    private readonly IRepository<SpaceUser, Guid> _spaceUserRepository;
    private readonly IAigcRepository<IdentityUser> _userRepository;
    public SpaceAppService(IRepository<Space, Guid> repository,
        IRepository<SpaceUser, Guid> spaceUserRepository,
        IAigcRepository<IdentityUser> userRepository,
        ISettingManager settingManager) : base(repository)
    {
        CreatePolicyName = AigcPermissions.Space.Create;
        UpdatePolicyName = AigcPermissions.Space.Update;
        DeletePolicyName = AigcPermissions.Space.Delete;
        GetPolicyName = AigcPermissions.Space.GetList;
        GetListPolicyName = AigcPermissions.Space.GetList;

        _settingManager = settingManager;
        _spaceUserRepository = spaceUserRepository;
        _userRepository = userRepository;
    }

    [HttpPost]
    [Route("[action]")]
    public virtual async Task SetCurrentSpaceAsync(Guid spaceId)
    {
        await _settingManager.SetForCurrentUserAsync(AigcSettingConst.CurrentSpaceIdName, spaceId.ToString());
    }

    [HttpPost]
    [Route("[action]")]
    public virtual async Task<List<GetSpaceUserResponseDto>> GetSpaceUserListAsync(GetSpaceUserRequestDto request)
    {
        var spaceUserQuery = (await _spaceUserRepository.GetQueryableAsync())
            .Where(x => x.SpaceId == request.SpaceId);
        var userQuery = (await _userRepository.GetQueryableAsync())
            .WhereIf(!string.IsNullOrWhiteSpace(request.UserName), x => x.UserName.Contains(request.UserName));

        var result = (from spaceUser in spaceUserQuery
                      join user in userQuery on spaceUser.UserId equals user.Id
                      select new GetSpaceUserResponseDto
                      {
                          Id = spaceUser.Id,
                          SpaceId = spaceUser.SpaceId,
                          IsOwner = spaceUser.IsOwner,
                          CreationTime = spaceUser.CreationTime,
                          UserId = user.Id,
                          UserName = user.UserName,
                      }).ToList();

        return result;
    }
}
