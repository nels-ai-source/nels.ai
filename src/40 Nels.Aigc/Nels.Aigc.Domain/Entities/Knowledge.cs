using Nels.Aigc.Consts;
using Nels.Aigc.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities.Auditing;

namespace Nels.Aigc.Entities;

public class Knowledge : AuditedEntity<Guid>
{
    public Knowledge() { }
    public Knowledge(Guid id) : base(id) { }

    [Required]
    public virtual Guid SpaceId { get; set; } = Guid.Empty;

    [Required]
    [MaxLength(KnowledgeConsts.MaxNameLength)]
    public virtual string Name { get; set; } = default!;

    [MaxLength(KnowledgeConsts.MaxDescriptionLength)]
    public virtual string Description { get; set; } = default!;

    [Required]
    public virtual int DocumentCount { get; set; } = default!;

    [Required]
    public virtual int Length { get; set; } = default!;

    [Required]
    public virtual int AgentUseCount { get; set; } = default!;

    [Required]
    public virtual int RetrievalCount { get; set; } = default!;

    public virtual Guid? ModelId { get; set; }

    public virtual SearchType SearchType { get; set; }

    [Required]
    public virtual bool IsEnabled { get; set; } = true;
}