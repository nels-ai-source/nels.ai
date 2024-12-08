using Nels.Abp.Ddd.Application.Contracts.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Validation;

namespace Nels.Abp.SysMng.Identity;

public class OrganizationUnitDto : AuditedEntityDto<Guid>, IHasChildrenDto<OrganizationUnitDto, Guid>
{
    [Required]
    [DynamicStringLength(typeof(OrganizationUnitConsts), nameof(OrganizationUnitConsts.MaxCodeLength))]
    public string Code { get; set; } = string.Empty;

    [DynamicStringLength(typeof(OrganizationUnitConsts), nameof(OrganizationUnitConsts.MaxDisplayNameLength))]
    public string DisplayName { get; set; } = string.Empty;

    public Guid? ParentId { get; set; }

    public virtual List<OrganizationUnitDto>? Children { get; set; }
}
