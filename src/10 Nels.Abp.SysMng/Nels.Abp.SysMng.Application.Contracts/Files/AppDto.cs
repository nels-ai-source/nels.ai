using Nels.Abp.SysMng.FunctionPage;
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

    [Required]
    [StringLength(FileConsts.MaxNameLength)]
    public virtual string Type { get; set; } = string.Empty;

    [Required]
    [StringLength(FileConsts.MaxPathLength)]
    public virtual string Path { get; set; } = string.Empty;
}
