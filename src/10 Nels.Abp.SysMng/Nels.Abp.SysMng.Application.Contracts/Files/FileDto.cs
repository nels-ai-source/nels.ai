using Nels.Abp.SysMng.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;

namespace Nels.Abp.SysMng.Files;

public class FileDto : AuditedEntityDto<Guid>
{
    [Required]
    [StringLength(FileConsts.MaxNameLength)]
    public virtual string Name { get; set; } = string.Empty;

    [Required]
    public virtual long Size { get; set; } = 0;

    public virtual DocumentType Type { get; set; }

    [Required]
    [StringLength(FileConsts.MaxPathLength)]
    public virtual string Path { get; set; } = string.Empty;
}
