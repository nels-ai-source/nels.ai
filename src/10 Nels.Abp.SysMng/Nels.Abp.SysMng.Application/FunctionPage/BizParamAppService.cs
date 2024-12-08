using Microsoft.AspNetCore.Mvc;
using Nels.Abp.Ddd.Application.Services;
using Nels.Abp.SysMng.Permissions;
using System;
using Volo.Abp.Domain.Repositories;

namespace Nels.Abp.SysMng.FunctionPage;

/// <summary>
/// 业务参数服务类
/// </summary>
[Route(SysMngRemoteServiceConsts.BizParamRoute)]
public class BizParamAppService : RouteCrudAppService<BizParameter, BizParameterDto, Guid>
{
    private IRepository<Page, Guid> MenuRepository;
    public BizParamAppService(IRepository<BizParameter, Guid> repository, IRepository<Page, Guid> menuRepository) : base(repository)
    {
        CreatePolicyName = SysMngPermissions.BizParameter.Create;
        UpdatePolicyName = SysMngPermissions.BizParameter.Update;
        DeletePolicyName = SysMngPermissions.BizParameter.Delete;
        GetPolicyName = SysMngPermissions.BizParameter.GetList;
        GetListPolicyName = SysMngPermissions.BizParameter.GetList;

        MenuRepository = menuRepository;
    }

}
