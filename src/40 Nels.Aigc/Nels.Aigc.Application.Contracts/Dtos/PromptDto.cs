using Nels.Aigc.Consts;
using Nels.SemanticKernel;
using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;

namespace Nels.Aigc.Dtos;

public class PromptDto : AuditedEntityDto<Guid>, IPrompt
{
    [Required]
    public virtual string Template { get; set; } = string.Empty;

    [StringLength(PromptConsts.MaxNameLength)]
    public virtual string Name { get; set; } = string.Empty;

    [StringLength(PromptConsts.MaxDescriptionLength)]
    public virtual string Description { get; set; } = string.Empty;

    [Required]
    [StringLength(PromptConsts.MaxTemplateFormatLength)]
    public virtual string TemplateFormat { get; set; } = string.Empty;
}
