using Nels.Aigc.Consts;
using Nels.SemanticKernel.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;

namespace Nels.Aigc.Entities;

public class Model : AuditedEntity<Guid>
{
    public Model() { }
    public Model(Guid id) : base(id) { }

    [Required]
    public virtual ModelProvider Provider { get; set; }

    [Required]
    public virtual ModelType Type { get; set; }

    [Required]
    [MaxLength(ModelConsts.MaxNameLength)]
    public virtual string Name { get; set; } = string.Empty;

    [MaxLength(ModelConsts.MaxEndpointLength)]
    public virtual string Endpoint { get; set; } = string.Empty;


    [MaxLength(ModelConsts.MaxPropertiesLength)]
    public virtual string Properties { get; set; } = string.Empty;
}
