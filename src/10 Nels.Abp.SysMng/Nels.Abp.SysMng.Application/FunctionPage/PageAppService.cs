using Microsoft.AspNetCore.Mvc;
using Nels.Abp.Ddd.Application.Services;
using Nels.Abp.SysMng.Permissions;
using System;
using Volo.Abp.Domain.Repositories;

namespace Nels.Abp.SysMng.FunctionPage;

/// <summary>
/// 页面服务类
/// </summary>
[Route(SysMngRemoteServiceConsts.pageRoute)]
public class PageAppService : RouteCrudGetAllAppService<Page, PageDto, Guid>
{
    public PageAppService(IRepository<Page, Guid> repository) : base(repository)
    {
        CreatePolicyName = SysMngPermissions.Page.Create;
        UpdatePolicyName = SysMngPermissions.Page.Update;
        DeletePolicyName = SysMngPermissions.Page.Delete;
        GetPolicyName = SysMngPermissions.Page.GetList;
        GetListPolicyName = SysMngPermissions.Page.GetList;
    }
}
