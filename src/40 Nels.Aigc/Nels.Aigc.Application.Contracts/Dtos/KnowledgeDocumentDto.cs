using Nels.Abp.SysMng.Enums;
using Nels.Aigc.Consts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;

namespace Nels.Aigc.Dtos
{
    public class KnowledgeDocumentDto : AuditedEntityDto<Guid>
    {
        [Required]
        public virtual Guid KnowledgeId { get; set; }

        public virtual Guid? FileId { get; set; }

        [Required]
        [StringLength(KnowledgeDocumentConsts.MaxNameLength)]
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

        public virtual List<KnowledgeDocumentParagraphDto> KnowledgeDocumentParagraphs { get; set; } = [];
    }
    public class KnowledgeDocumentParagraphDto : AuditedEntityDto<Guid>
    {
        [Required]
        public virtual Guid KnowledgeId { get; set; }

        [Required]
        public virtual Guid KnowledgeDocumentId { get; set; }

        [Required]
        public virtual int Index { get; set; } = default!;

        [StringLength(KnowledgeDocumentParagraphConsts.MaxContentLength)]
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

    public class AddKnowledgeDocumentRequest 
    {
        [Required]
        public virtual Guid KnowledgeId { get; set; }

        public virtual Guid FileId { get; set; }
    }
}
