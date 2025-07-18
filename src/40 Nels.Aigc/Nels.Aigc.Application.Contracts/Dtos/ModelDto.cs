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
    public virtual ModelConnector Connector { get; set; }

    [Required]
    [StringLength(ModelConsts.MaxNameLength)]
    public virtual string Name { get; set; } = string.Empty;

    public virtual string Endpoint { get; set; } = string.Empty;

    public virtual string AccessKey { get; set; } = string.Empty;

    public virtual string SecretKey { get; set; } = string.Empty;

    public virtual bool IsEnabled { get; set; }

    public virtual string DeploymentName { get; set; } = string.Empty;

    public virtual int? MaxTokens { get; set; }
    public virtual List<ModelCapability> ModelCapabilities { get; set; }
}
public class ModelUpdateInputDto : EntityDto
{
    [Required]
    public virtual ModelType Type { get; set; }
    [Required]
    [StringLength(ModelConsts.MaxNameLength)]
    public virtual string Name { get; set; } = string.Empty;

    public string DeploymentName { get; set; } = string.Empty;
    public string Endpoint { get; set; } = string.Empty;
    public virtual int? MaxTokens { get; set; }
    public virtual List<ModelCapability> ModelCapabilities { get; set; }
}
public class ModelGetListInputDto
{
    public virtual string? Keyword { get; set; }
    public virtual ModelProvider? Provider { get; set; }
    public virtual ModelType? Type { get; set; }
    public virtual int? MaxTokens { get; set; }
    public virtual List<ModelCapability>? ModelCapabilities { get; set; }

}
public class ModelGetListOutputDto : AuditedEntityDto<Guid>
{
    public virtual ModelProvider Provider { get; set; }
    public virtual ModelType Type { get; set; }
    public virtual ModelConnector ModelConnector { get; set; }
    public virtual string Name { get; set; }
    public virtual string Endpoint { get; set; }
    public virtual bool IsEnabled { get; set; }
    public virtual int? MaxTokens { get; set; }
    public virtual List<ModelCapability> ModelCapabilities { get; set; }
}

public class ModelSetKeyDto
{
    public virtual ModelProvider? Provider { get; set; }
    public virtual Guid? Id { get; set; }
    public virtual string? AccessKey { get; set; }
    public virtual string? SecretKey { get; set; }
}