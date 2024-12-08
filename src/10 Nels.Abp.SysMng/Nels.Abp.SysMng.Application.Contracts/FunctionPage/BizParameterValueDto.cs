using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;

namespace Nels.Abp.SysMng.FunctionPage;

public class BizParameterValueDto : AuditedEntityDto<Guid>
{
    /// <summary>
    /// 名称
    /// </summary>
    [StringLength(BizParameterValueConsts.MaxTextLength)]
    [Required]
    public virtual string Text { get; set; } = string.Empty;
    /// <summary>
    /// 显示名称
    /// </summary>
    [StringLength(BizParameterValueConsts.MaxValueLength)]
    public virtual string Value { get; set; } = string.Empty;
    /// <summary>
    /// 值
    /// </summary>
    [StringLength(BizParameterValueConsts.MaxValueLength)]
    public virtual string? ParentValue { get; set; } = string.Empty;
}
