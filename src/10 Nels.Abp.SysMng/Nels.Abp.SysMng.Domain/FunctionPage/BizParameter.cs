using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities.Auditing;

namespace Nels.Abp.SysMng.FunctionPage;

public class BizParameter : AuditedEntity<Guid>
{
    [Required]
    [MaxLength(BizParameterConsts.MaxNameLength)]
    public virtual string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(BizParameterConsts.MaxDisplayNameLength)]
    public virtual string DisplayName { get; set; } = string.Empty;

    public virtual bool IsEnabled { get; set; } = true;

    [MaxLength(BizParameterConsts.MaxValueLength)]
    public virtual string Value { get; set; } = string.Empty;

    public virtual Guid BusinessUnitId { get; set; }

    [Required]
    public virtual BizParamTypeEnum TypeEnum { get; set; }

    [Required]
    public virtual BizParamValueTypeEnum ValueTypeEnum { get; set; }

    [MaxLength(BizParameterConsts.MaxDisplayNameLength)]
    public string Description { get; set; } = string.Empty;
}
