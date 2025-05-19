using Nels.Aigc.Consts;
using Nels.SemanticKernel;
using Nels.SemanticKernel.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;

namespace Nels.Aigc.Dtos;

public class ModelDto : AuditedEntityDto<Guid>, IModel
{
    [Required]
    public virtual ModelProvider Provider { get; set; }

    [Required]
    public virtual ModelType Type { get; set; }

    [Required]
    public ModelConnector ModelConnector { get; set; }

    [Required]
    [StringLength(ModelConsts.MaxNameLength)]
    public virtual string Name { get; set; } = string.Empty;

    [StringLength(ModelConsts.MaxPropertiesLength)]
    public virtual string Properties { get; set; } = string.Empty;
    public string DeploymentName { get; set; } = string.Empty;
    public string Endpoint { get; set; } = string.Empty;
    public string AccessKey { get; set; } = string.Empty;
    public string SecretKey { get; set; } = string.Empty;
    public bool IsDefault { get; set; }

}
public class ModelGetListInputDto
{
    public virtual string? Name { get; set; }
}
public class ModelGetListOutputDto : EntityDto<Guid>
{
    public virtual ModelProvider Provider { get; set; }
    public virtual ModelType Type { get; set; }
    public virtual ModelConnector ModelConnector { get; set; }
    public virtual string Name { get; set; }
    public virtual string Endpoint { get; set; }
    public virtual bool IsEnabled { get; set; }
    public virtual List<ModelCapability> ModelCapabilities { get; set; }
}

public class ModelSettingDto
{
    public virtual ModelProvider Provider { get; set; }
    public virtual string? Endpoint { get; set; }
    public virtual string AccessKey { get; set; }
    public virtual string? SecretKey { get; set; }
    public virtual string? DeploymentName { get; set; }
}

public class ModelSetKeyDto
{
    public virtual List<Guid> Ids { get; set; }
    public virtual string? AccessKey { get; set; }
    public virtual string? SecretKey { get; set; }
}