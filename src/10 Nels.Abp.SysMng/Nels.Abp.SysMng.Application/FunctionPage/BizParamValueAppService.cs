using Microsoft.AspNetCore.Mvc;
using Nels.Abp.Ddd.Application.Services;
using Nels.Abp.SysMng.Permissions;
using System;
using Volo.Abp.Domain.Repositories;

namespace Nels.Abp.SysMng.FunctionPage;

/// <summary>
/// 业务参数值服务类
/// </summary>
[Route(SysMngRemoteServiceConsts.BizParamValueRoute)]
public class BizParamValueAppService : RouteCrudAppService<BizParameterValue, BizParameterValueDto, Guid>
{
    public BizParamValueAppService(IRepository<BizParameterValue, Guid> repository) : base(repository)
    {
        CreatePolicyName = SysMngPermissions.BizParameteValue.Create;
        UpdatePolicyName = SysMngPermissions.BizParameteValue.Update;
        DeletePolicyName = SysMngPermissions.BizParameteValue.Delete;
        GetPolicyName = SysMngPermissions.BizParameteValue.GetList;
        GetListPolicyName = SysMngPermissions.BizParameteValue.GetList;
    }
}
