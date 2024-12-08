using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities.Auditing;

namespace Nels.Abp.SysMng.FunctionPage;

public class BizParameterValue : AuditedEntity<Guid>
{
    [Required]
    public virtual Guid BizParameterId { get; set; }

    [Required]
    [MaxLength(BizParameterValueConsts.MaxTextLength)]
    public virtual string Text { get; set; } = string.Empty;

    [Required]
    [MaxLength(BizParameterValueConsts.MaxValueLength)]
    public virtual string Value { get; set; } = string.Empty;

    [MaxLength(BizParameterValueConsts.MaxValueLength)]
    public virtual string? ParentValue { get; set; }
}
