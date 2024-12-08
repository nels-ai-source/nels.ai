using Nels.Aigc.Consts;
using Nels.Aigc.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;

namespace Nels.Aigc.Entities;

public class KnowledgeDocument : AuditedEntity<Guid>, IAggregateRoot<Guid>
{
    public KnowledgeDocument() { }
    public KnowledgeDocument(Guid id, string name, DocumentType documentType) : base(id)
    {
        Name = name;
        DocumentType = documentType;
    }

    [Required]
    public virtual Guid KnowledgeId { get; set; }

    [Required]
    [MaxLength(KnowledgeDocumentConsts.MaxNameLength)]
    public virtual string Name { get; set; } = default!;

    public virtual DocumentType DocumentType { get; set; }

    [Required]
    public virtual int Length { get; set; } = default!;

    [Required]
    public virtual int RetrievalCount { get; set; } = default!;

    [Required]
    public virtual int ParagraphCount { get; set; } = default!;

    [Required]
    public virtual bool IsEnabled { get; set; } = true;

    public virtual List<KnowledgeDocumentParagraph> KnowledgeDocumentParagraphs { get; set; } = [];
}

public class KnowledgeDocumentParagraph : AuditedEntity<Guid>
{
    protected KnowledgeDocumentParagraph() { }
    internal KnowledgeDocumentParagraph(Guid id, int index, string content, bool isEnable = true) : base(id)
    {
        Index = index;
        Content = content;
        IsEnabled = isEnable;
    }

    [Required]
    public virtual Guid KnowledgeId { get; set; }

    [Required]
    public virtual Guid KnowledgeDocumentId { get; set; }

    [Required]
    public virtual int Index { get; set; } = default!;

    [MaxLength(KnowledgeDocumentParagraphConsts.MaxContentLength)]
    [Required]
    public virtual string Content { get; set; } = default!;

    [Required]
    public virtual bool IsEnabled { get; set; } = true;

    [Required]
    public virtual int RetrievalCount { get; set; } = default!;

    [Required]
    public virtual int Length => this.Content.Length;

    [Required]
    public virtual bool Embedding { get; set; } = default!;
}
