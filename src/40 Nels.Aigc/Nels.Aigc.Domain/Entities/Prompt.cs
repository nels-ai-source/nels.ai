using Nels.Aigc.Consts;
using Nels.SemanticKernel;
using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities.Auditing;

namespace Nels.Aigc.Entities;

public class Prompt : AuditedEntity<Guid>, IPrompt
{
    public Prompt() { }
    public Prompt(Guid id) : base(id) { }

    [Required]
    [MaxLength(PromptConsts.MaxNameLength)]
    public virtual string Name { get; set; } = string.Empty;

    [MaxLength(PromptConsts.MaxDescriptionLength)]
    public virtual string Description { get; set; } = string.Empty;

    [Required]
    public virtual string Template { get; set; } = string.Empty;

    [Required]
    [MaxLength(PromptConsts.MaxTemplateFormatLength)]
    public virtual string TemplateFormat { get; set; } = string.Empty;
}
