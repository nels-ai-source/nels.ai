using Nels.Abp.SysMng.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities.Auditing;

namespace Nels.Abp.SysMng.Files;

public class FileEntity : AuditedEntity<Guid>
{
    public FileEntity() { }

    public FileEntity(Guid id) : base(id) { }

    [Required]
    [MaxLength(FileConsts.MaxNameLength)]
    public virtual string Name { get; set; }

    [Required]
    public virtual long Size { get; set; } = 0;

    public virtual DocumentType Type { get; set; }

    [MaxLength(FileConsts.MaxMimeTypeLength)]
    public virtual string MimeType { get; set; }

    [Required]
    [MaxLength(FileConsts.MaxPathLength)]
    public virtual string Path { get; set; }
}
