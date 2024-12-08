using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;

namespace Nels.Abp.SysMng.FunctionPage;

/// <summary>
/// 页面
/// </summary>
public class Page : AuditedEntity<Guid>, IAggregateRoot<Guid>, IHasOrderCode
{
    public Page() { }
    public Page(Guid id) : base(id) { }
    /// <summary>
    /// 应用ID
    /// </summary>
    [Required]
    public virtual Guid ApplicationId { get; set; }
    /// <summary>
    /// 业务单元ID
    /// </summary>
    [Required]
    public virtual Guid BusinessUnitId { get; set; }
    /// <summary>
    /// 名称
    /// </summary>
    [Required]
    [MaxLength(PageConsts.MaxNameLength)]
    public virtual string Name { get; set; } = string.Empty;
    /// <summary>
    /// 路径
    /// </summary>
    [MaxLength(PageConsts.MaxPathLength)]
    public virtual string Path { get; set; } = string.Empty;

    #region meta
    /// <summary>
    /// 显示名称。展示在菜单，标签和面包屑等中
    /// </summary>
    [Required]
    [MaxLength(PageConsts.MaxDisplayNameLength)]
    public virtual string DisplayName { get; set; } = string.Empty;
    /// <summary>
    /// 是否隐藏菜单，大部分用在无需显示在左侧菜单中的页面，比如详情页
    /// </summary>
    [Required]
    public virtual bool Hidden { get; set; } = false;
    /// <summary>
    /// 是否固定，类似首页控制台在标签中是没有关闭按钮的
    /// </summary>
    [Required]
    public virtual bool Affix { get; set; } = false;
    /// <summary>
    /// 显示图标，建立2级菜单都设置图标，否则菜单折叠都将显示空白
    /// </summary>
    [Required]
    [MaxLength(PageConsts.MaxIconLength)]
    public virtual string Icon { get; set; } = string.Empty;
    /// <summary>
    /// 类型：菜单，Iframe，外链，按钮（menu、iframe、link）
    /// </summary>
    [Required]
    public virtual PageTypeEnum Type { get; set; } = PageTypeEnum.Menu;
    /// <summary>
    /// 是否隐藏面包屑
    /// </summary>
    [Required]
    public virtual bool HiddenBreadcrumb { get; set; } = false;
    /// <summary>
    /// 左侧菜单的路由地址活动状态，比如打开详情页需要列表页的菜单活动状态
    /// </summary>
    [MaxLength(PageConsts.MaxActiveLength)]
    public virtual string Active { get; set; } = string.Empty;
    /// <summary>
    /// 是否整页打开路由（脱离框架系）
    /// </summary>
    [Required]
    public virtual bool Fullpage { get; set; } = false;
    #endregion
    /// <summary>
    /// 描述
    /// </summary>
    [MaxLength(PageConsts.MaxDescriptionLength)]
    public virtual string Description { get; set; } = string.Empty;
    /// <summary>
    /// 排序字段
    /// </summary>
    [MaxLength(PageConsts.MaxSortLength)]
    public virtual string Sort { get; set; } = string.Empty;
    /// <summary>
    /// 权限
    /// </summary>
    [MaxLength(PageConsts.MaxPermissionLength)]
    public virtual string Permission { get; set; } = string.Empty;
}
