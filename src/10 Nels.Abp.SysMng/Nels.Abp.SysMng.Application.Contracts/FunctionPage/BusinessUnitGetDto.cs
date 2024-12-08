using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;

namespace Nels.Abp.SysMng.FunctionPage;

public class BusinessUnitDto : AuditedEntityDto<Guid>
{

    [Required]
    public virtual Guid ApplicationId { get; set; }

    [Required]
    [StringLength(BusinessUnitConsts.MaxNameLength)]
    public virtual string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(BusinessUnitConsts.MaxDisplayNameLength)]
    public virtual string DisplayName { get; set; } = string.Empty;

    [StringLength(BusinessUnitConsts.MaxDescriptionLength)]
    public virtual string Description { get; set; } = string.Empty;

    [StringLength(BusinessUnitConsts.MaxIconLength)]
    public virtual string Icon { get; set; } = string.Empty;
    [Required]
    public virtual bool IsEnable { get; set; } = true;

    [StringLength(BusinessUnitConsts.MaxSortLength)]
    public virtual string Sort { get; set; } = string.Empty;
}
