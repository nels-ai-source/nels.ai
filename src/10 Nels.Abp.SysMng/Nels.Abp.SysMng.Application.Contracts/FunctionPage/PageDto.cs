using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;

namespace Nels.Abp.SysMng.FunctionPage;

public class PageDto : AuditedEntityDto<Guid>
{
    [Required]
    public virtual Guid ApplicationId { get; set; }

    [Required]
    public virtual Guid BusinessUnitId { get; set; }

    [StringLength(PageConsts.MaxNameLength)]
    [Required]
    public virtual string Name { get; set; } = string.Empty;


    [StringLength(PageConsts.MaxPathLength)]
    public virtual string Path { get; set; } = string.Empty;

    [Required]
    [StringLength(PageConsts.MaxDisplayNameLength)]
    public virtual string DisplayName { get; set; } = string.Empty;

    [Required]
    public virtual bool Hidden { get; set; } = false;

    [Required]
    public virtual bool Affix { get; set; } = false;

    [Required]
    [StringLength(PageConsts.MaxIconLength)]
    public virtual string Icon { get; set; } = string.Empty;

    [Required]
    public virtual PageTypeEnum Type { get; set; } = PageTypeEnum.Menu;

    [Required]
    public virtual bool HiddenBreadcrumb { get; set; } = false;

    [StringLength(PageConsts.MaxActiveLength)]
    public virtual string Active { get; set; } = string.Empty;

    [Required]
    public virtual bool Fullpage { get; set; } = false;

    /// <summary>
    /// 描述
    /// </summary>
    [StringLength(PageConsts.MaxDescriptionLength)]
    public virtual string Description { get; set; } = string.Empty;
    /// <summary>
    /// 排序字段
    /// </summary>
    [StringLength(PageConsts.MaxSortLength)]
    public virtual string Sort { get; set; } = string.Empty;
    /// <summary>
    /// 权限
    /// </summary>
    [StringLength(PageConsts.MaxPermissionLength)]
    public virtual string Permission { get; set; } = string.Empty;
}
