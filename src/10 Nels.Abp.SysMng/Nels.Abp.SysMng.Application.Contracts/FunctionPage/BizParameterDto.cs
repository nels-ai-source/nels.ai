using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;

namespace Nels.Abp.SysMng.FunctionPage;

public class BizParameterDto : AuditedEntityDto<Guid>
{
    [StringLength(BizParameterConsts.MaxNameLength)]
    [Required]
    public virtual string Name { get; set; } = string.Empty;

    [StringLength(BizParameterConsts.MaxDisplayNameLength)]
    public virtual string DisplayName { get; set; } = string.Empty;

    public virtual bool IsEnabled { get; set; }

    [StringLength(BizParameterConsts.MaxValueLength)]
    public virtual string Value { get; set; } = string.Empty;

    public virtual Guid BusinessUnitId { get; set; }

    public virtual BizParamTypeEnum TypeEnum { get; set; }

    public virtual BizParamValueTypeEnum ValueTypeEnum { get; set; }

    public virtual string Description { get; set; } = string.Empty;
}
