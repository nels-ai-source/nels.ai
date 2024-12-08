using Nels.Abp.Ddd.Application.Contracts.Contracts;
using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Nels.Abp.SysMng.FunctionPage;

public class MenuDto : AuditedEntityDto<Guid>, IHasChildrenDto<MenuDto, Guid>, IHasOrderCode
{
    /// <summary>
    /// 路由标识
    /// </summary>
    public virtual string Name { get; set; } = string.Empty;
    /// <summary>
    /// 路径
    /// </summary>
    public virtual string Path { get; set; } = string.Empty;
    /// <summary>
    /// 组件
    /// </summary>
    public virtual string Component { get; set; } = string.Empty;
    /// <summary>
    /// 上级ID
    /// </summary>
    public virtual Guid? ParentId { get; set; }
    /// <summary>
    /// 元数据
    /// </summary>
    public virtual MenuMetaDto Meta { get; set; } = new MenuMetaDto();
    /// <summary>
    /// 子级菜单
    /// </summary>
    public virtual List<MenuDto>? Children { get; set; }
    /// <summary>
    /// 描述
    /// </summary>
    public virtual string Description { get; set; } = string.Empty;
    /// <summary>
    /// 排序编码
    /// </summary>
    public string Sort { get; set; } = string.Empty;
}
public class MenuMetaDto
{
    /// <summary>
    /// 显示名称。展示在菜单，标签和面包屑等中
    /// </summary>
    public virtual string Title { get; set; } = string.Empty;
    /// <summary>
    /// 是否隐藏菜单，大部分用在无需显示在左侧菜单中的页面，比如详情页
    /// </summary>
    public virtual bool Hidden { get; set; } = false;
    /// <summary>
    /// 是否固定，类似首页控制台在标签中是没有关闭按钮的
    /// </summary>
    public virtual bool Affix { get; set; } = false;
    /// <summary>
    /// 显示图标，建立2级菜单都设置图标，否则菜单折叠都将显示空白
    /// </summary>
    public virtual string Icon { get; set; } = string.Empty;
    /// <summary>
    /// 类型：菜单，Iframe，外链，按钮
    /// </summary>
    public virtual string Type { get; set; } = string.Empty;
    /// <summary>
    /// 是否隐藏面包屑
    /// </summary>
    public virtual bool HiddenBreadcrumb { get; set; } = false;
    /// <summary>
    /// 左侧菜单的路由地址活动状态，比如打开详情页需要列表页的菜单活动状态
    /// </summary>
    public virtual string Active { get; set; } = string.Empty;
    /// <summary>
    /// 颜色值
    /// </summary>
    public virtual string Color { get; set; } = string.Empty;
    /// <summary>
    /// 是否整页打开路由（脱离框架系）
    /// </summary>
    public virtual bool Fullpage { get; set; } = false;
    /// <summary>
    /// 静态路由时，所能访问路由的角色
    /// </summary>
    public virtual string Role { get; set; } = string.Empty;
}
