using Nels.Aigc.Consts;
using Nels.SemanticKernel;
using Nels.SemanticKernel.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;

namespace Nels.Aigc.Dtos;

public class ModelInstanceDto : AuditedEntityDto<Guid>, IModelInstance
{
    [Required]
    public virtual Guid ModelId { get; set; }

    [Required]
    [StringLength(ModelInstanceConsts.MaxNameLength)]
    public virtual string Name { get; set; } = string.Empty;

    [StringLength(ModelInstanceConsts.MaxDescriptionLength)]
    public virtual string Description { get; set; } = string.Empty;

    [StringLength(ModelInstanceConsts.MaxEndpointLength)]
    public virtual string Endpoint { get; set; } = string.Empty;

    [StringLength(ModelInstanceConsts.MaxAccessKeyLength)]
    public virtual string AccessKey { get; set; } = string.Empty;

    [StringLength(ModelInstanceConsts.MaxSecretKeyLength)]
    public virtual string SecretKey { get; set; } = string.Empty;

    [Required]
    public virtual bool IsDefault { get; set; }

    [StringLength(ModelInstanceConsts.MaxDeploymentNameLength)]
    public virtual string DeploymentName { get; set; } = string.Empty;

    [Required]
    public virtual ModelProvider Provider { get; set; }

    [Required]
    public virtual ModelType Type { get; set; }
}

public class ModelInstanceCreateInputDto
{
    [Required]
    public virtual Guid ModelId { get; set; }

    [StringLength(ModelInstanceConsts.MaxEndpointLength)]
    public virtual string? Endpoint { get; set; } = string.Empty;

    [StringLength(ModelInstanceConsts.MaxAccessKeyLength)]
    public virtual string? AccessKey { get; set; } = string.Empty;

    [StringLength(ModelInstanceConsts.MaxSecretKeyLength)]
    public virtual string? SecretKey { get; set; } = string.Empty;

    [StringLength(ModelInstanceConsts.MaxDeploymentNameLength)]
    public virtual string? DeploymentName { get; set; } = string.Empty;

    [StringLength(ModelInstanceConsts.MaxDescriptionLength)]
    public virtual string? Description { get; set; } = string.Empty;
}
public class ModelInstanceUpdateInputDto
{

    [StringLength(ModelInstanceConsts.MaxEndpointLength)]
    public virtual string? Endpoint { get; set; } = string.Empty;

    [StringLength(ModelInstanceConsts.MaxDeploymentNameLength)]
    public virtual string? DeploymentName { get; set; } = string.Empty;

    [StringLength(ModelInstanceConsts.MaxDescriptionLength)]
    public virtual string? Description { get; set; } = string.Empty;
}
public class ModelInstanceGetListInputDto : ILimitedResultRequest
{
    public virtual string? Name { get; set; }

    public virtual ModelProvider? ModelProvider { get; set; }

    public virtual Guid? ModelId { get; set; }
    public virtual int MaxResultCount { get; set; } = 100;
}
public class ModelInstanceGetListOutputDto : EntityDto<Guid>
{
    public virtual string Name { get; set; }
    public virtual string DeploymentName { get; set; }
    public virtual ModelProvider Provider { get; set; }
    public virtual string ProviderName { get; set; }
    public virtual ModelType Type { get; set; }
    public virtual string TypeName { get; set; }
    public virtual bool IsDefault { get; set; }
    public virtual string Endpoint { get; set; }
    public virtual string Description { get; set; }
}
public class ModelInstanceSetKeyDto
{
    public virtual List<Guid> Ids { get; set; }
    public virtual string? AccessKey { get; set; }
    public virtual string? SecretKey { get; set; }
}