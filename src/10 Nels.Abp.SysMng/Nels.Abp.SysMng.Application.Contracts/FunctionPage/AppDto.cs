using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;

namespace Nels.Abp.SysMng.FunctionPage;

public class AppDto : AuditedEntityDto<Guid>
{
    [StringLength(AppConsts.MaxNameLength)]
    public virtual string Name { get; set; } = string.Empty;

    [StringLength(AppConsts.MaxDisplayNameLength)]
    public virtual string DisplayName { get; set; } = string.Empty;

    [StringLength(AppConsts.MaxDescriptionLength)]
    public virtual string Description { get; set; } = string.Empty;

    [StringLength(AppConsts.MaxIconLength)]
    public virtual string Icon { get; set; } = string.Empty;
    [Required]
    public virtual bool IsEnable { get; set; } = true;

    [StringLength(AppConsts.MaxSortLength)]
    public virtual string Sort { get; set; } = string.Empty;
}
