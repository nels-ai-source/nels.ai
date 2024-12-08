using Nels.Aigc.Consts;
using Nels.Aigc.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;

namespace Nels.Aigc.Dtos;

public class KnowledgeDto : AuditedEntityDto<Guid>
{
    [Required]
    public virtual Guid SpaceId { get; set; } = Guid.Empty;

    [Required]
    [StringLength(KnowledgeConsts.MaxNameLength)]
    public virtual string Name { get; set; } = default!;

    [StringLength(KnowledgeConsts.MaxDescriptionLength)]
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

    public virtual List<KnowledgeDocumentDto> KnowledgeDocuments { get; set; } = [];
}

