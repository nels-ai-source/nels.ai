using Nels.Aigc.Consts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;

namespace Nels.Aigc.Entities;

public class Plugin : FullAuditedEntity<Guid>, IAggregateRoot<Guid>
{
    public Plugin() { }
    public Plugin(Guid id) : base(id) { }
    public virtual Guid SpaceId { get; set; } = Guid.Empty;

    [Required]
    [MaxLength(PluginConsts.MaxNameLength)]
    public virtual string Name { get; set; } = default!;

    [MaxLength(PluginConsts.MaxDescriptionLength)]
    public virtual string Description { get; set; } = default!;

    [MaxLength(PluginConsts.MaxVersionLength)]
    public virtual string Version { get; set; } = default!;

    [Required]
    [MaxLength(PluginConsts.MaxManifestUrlLength)]

    public virtual string ManifestUrl { get; set; } = default!;

    [MaxLength(PluginConsts.MaxIconLength)]
    public virtual string Icon { get; set; } = default!;

    public virtual List<Tool> Tools { get; set; } = [];
}

public class Tool : Entity<Guid>
{
    public Tool() { }
    public Tool(Guid id) : base(id) { }
    public virtual Guid PluginId { get; set; } = Guid.Empty;

    [Required]
    [MaxLength(ToolConsts.MaxNameLength)]
    public virtual string Name { get; set; } = default!;

    [MaxLength(ToolConsts.MaxDescriptionLength)]
    public virtual string Description { get; set; } = default!;

    public virtual List<ToolParamter> InputParamters { get; set; } = [];
    public virtual List<ToolParamter> OutputParamters { get; set; } = [];
}

public class ToolParamter : Entity<Guid>
{
    public ToolParamter() { }
    public ToolParamter(Guid id) : base(id) { }

    [Required]
    public virtual Guid ToolId { get; set; }

    public virtual ParameterDirection ParameterDirection { get; set; }

    [Required]
    [MaxLength(ToolParamterConsts.MaxNameLength)]
    public virtual string Name { get; set; } = default!;

    [MaxLength(ToolParamterConsts.MaxDescriptionLength)]
    public virtual string Description { get; set; } = default!;

    [Required]
    [MaxLength(ToolParamterConsts.MaxTypeLength)]
    public virtual string Type { get; set; } = default!;

    public virtual bool Required { get; set; } = false;
}

public enum ParameterDirection
{
    Input = 1,
    Output = 2
}