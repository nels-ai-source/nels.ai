using Nels.Aigc.Consts;
using Nels.SemanticKernel.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;

namespace Nels.Aigc.Dtos;

public class ModelDto : AuditedEntityDto<Guid>
{
    [Required]
    public virtual ModelProvider Provider { get; set; }

    [Required]
    public virtual ModelType Type { get; set; }

    [Required]
    [StringLength(ModelConsts.MaxNameLength)]
    public virtual string Name { get; set; } = string.Empty;

    [StringLength(ModelConsts.MaxPropertiesLength)]
    public virtual string Properties { get; set; } = string.Empty;
}
public class ModelGetListInputDto
{
    public virtual string? Name { get; set; }
}
public class ModelGetListOutputDto : EntityDto<Guid>
{
    public virtual ModelProvider Provider { get; set; }
    public virtual string Name { get; set; }
    public virtual Guid? ParentId { get; set; }
    public virtual string Endpoint { get; set; }
    public virtual string Properties { get; set; }
    public virtual List<ModelGetListOutputDto> children { get; set; }
}