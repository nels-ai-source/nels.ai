using Microsoft.AspNetCore.Mvc;
using Nels.Abp.Ddd.Application.Services;
using Nels.Abp.SysMng.Permissions;
using System;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;

namespace Nels.Abp.SysMng.Identity;

/// <summary>
/// Organization Unit service class that manages organizational structure operations.
/// Provides CRUD operations and retrieval of organizational units within the system.
/// </summary>
[Route(SysMngRemoteServiceConsts.OrganizationUnitRoute)]
public class OrganizationUnitAppService : RouteCrudGetAllAppService<OrganizationUnit, OrganizationUnitDto, Guid>
{
    public OrganizationUnitAppService(IRepository<OrganizationUnit, Guid> repository) : base(repository)
    {
        CreatePolicyName = SysMngPermissions.OrganizationUnit.Create;
        UpdatePolicyName = SysMngPermissions.OrganizationUnit.Update;
        DeletePolicyName = SysMngPermissions.OrganizationUnit.Delete;
        GetPolicyName = SysMngPermissions.OrganizationUnit.GetList;
        GetListPolicyName = SysMngPermissions.OrganizationUnit.GetList;
    }
}
