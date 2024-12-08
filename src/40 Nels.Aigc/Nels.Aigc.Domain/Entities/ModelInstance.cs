using Nels.Aigc.Consts;
using Nels.SemanticKernel.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities.Auditing;

namespace Nels.Aigc.Entities;

public class ModelInstance : AuditedEntity<Guid>
{
    public ModelInstance() { }
    public ModelInstance(Guid id) : base(id) { }

    [Required]
    public virtual Guid ModelId { get; set; }

    [Required]
    [MaxLength(ModelInstanceConsts.MaxNameLength)]
    public virtual string Name { get; set; } = string.Empty;

    [MaxLength(ModelInstanceConsts.MaxDescriptionLength)]
    public virtual string Description { get; set; } = string.Empty;

    [MaxLength(ModelInstanceConsts.MaxEndpointLength)]
    public virtual string Endpoint { get; set; } = string.Empty;

    [MaxLength(ModelInstanceConsts.MaxAccessKeyLength)]
    public virtual string AccessKey { get; set; } = string.Empty;

    [MaxLength(ModelInstanceConsts.MaxSecretKeyLength)]
    public virtual string SecretKey { get; set; } = string.Empty;

    [Required]
    public virtual bool IsDefault { get; set; }

    [MaxLength(ModelInstanceConsts.MaxDeploymentNameLength)]
    public virtual string DeploymentName { get; set; } = string.Empty;

    [Required]
    public virtual ModelProvider Provider { get; set; }

    [Required]
    public virtual ModelType Type { get; set; }
}
