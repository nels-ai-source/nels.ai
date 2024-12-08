using Microsoft.AspNetCore.Mvc;
using Nels.Abp.Ddd.Application.Contracts;
using Nels.Abp.Ddd.Application.Services;
using Nels.Abp.SysMng.Permissions;
using System;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using System.Linq;
using System.Collections.Generic;

namespace Nels.Abp.SysMng.FunctionPage;

/// <summary>
/// 业务单元服务类
/// </summary>
[Route(SysMngRemoteServiceConsts.businessUnitRoute)]
public class BusinessUnitAppService : RouteCrudGetAllAppService<BusinessUnit, BusinessUnitDto, BusinessUnitOutputDto, Guid, BusinessUnitGetDto, BusinessUnitDto, BusinessUnitDto>
{
    private readonly IRepository<App, Guid> _appRepository;
    public BusinessUnitAppService(IRepository<BusinessUnit, Guid> repository, IRepository<App, Guid> appRepository) : base(repository)
    {
        CreatePolicyName = SysMngPermissions.BusinessUnit.Create;
        UpdatePolicyName = SysMngPermissions.BusinessUnit.Update;
        DeletePolicyName = SysMngPermissions.BusinessUnit.Delete;
        GetPolicyName = SysMngPermissions.BusinessUnit.GetList;
        GetListPolicyName = SysMngPermissions.BusinessUnit.GetList;

        _appRepository = appRepository;
    }
    [HttpPost]
    [Route(RemoteServiceConsts.ApiRouteGetAllList)]
    public override async Task<List<BusinessUnitOutputDto>> GetAllListAsync(BusinessUnitGetDto input)
    {
        await CheckGetListPolicyAsync();

        var query = await CreateFilteredQueryAsync(input);
        var appQuery = await _appRepository.GetQueryableAsync();

        var appEntities = await AsyncExecuter.ToListAsync(appQuery);
        var appEntityDtos = MapList<App, BusinessUnitOutputDto>(appEntities);

        query = ApplySorting(query, input);

        var entities = await AsyncExecuter.ToListAsync(query);
        var entityDtos = await MapToGetListOutputDtosAsync(entities);

        foreach (var app in appEntityDtos)
        {
            app.Children = entityDtos.Where(x => x.ParentId == app.Id).ToList();
        }
        return appEntityDtos;
    }
}
