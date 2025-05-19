using Nels.Aigc.Consts;
using Nels.SemanticKernel.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
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
    public virtual ModelConnector Connector { get; set; }

    [Required]
    [MaxLength(ModelConsts.MaxNameLength)]
    public virtual string Name { get; set; } = string.Empty;

    [MaxLength(ModelConsts.MaxEndpointLength)]
    public virtual string Endpoint { get; set; } = string.Empty;

    [MaxLength(ModelInstanceConsts.MaxAccessKeyLength)]
    public virtual string AccessKey { get; set; } = string.Empty;

    [MaxLength(ModelInstanceConsts.MaxSecretKeyLength)]
    public virtual string SecretKey { get; set; } = string.Empty;

    [Required]
    public virtual bool IsEnabled { get; set; }

    [MaxLength(ModelInstanceConsts.MaxDeploymentNameLength)]
    public virtual string DeploymentName { get; set; } = string.Empty;

    [MaxLength(ModelConsts.MaxCapabilitiesLength)]
    public virtual string? Capabilities { get; set; }

    [NotMapped]
    public virtual List<ModelCapability> ModelCapabilities
    {
        get => string.IsNullOrEmpty(Capabilities)
            ? []
            : Capabilities.Split(';').Select(x => Enum.Parse<ModelCapability>(x.Trim('[', ']'))).ToList();
        set => Capabilities = string.Join(";", value.Select(x => $"[{(int)x}]"));
    }
}
