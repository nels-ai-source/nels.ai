using Nels.Aigc.Consts;
using Nels.Aigc.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.Guids;

namespace Nels.Aigc.Entities;

public class AgentInstance : AuditedEntity<Guid>, IAggregateRoot<Guid>
{
    public AgentInstance() { }
    public AgentInstance(Guid id) : base(id) { }

    [Required]
    public virtual Guid AgentId { get; set; }

    [Required]
    [MaxLength(AgentInstanceConsts.MaxTitleLength)]
    public virtual string Title { get; set; }

    [MaxLength(AgentInstanceConsts.MaxFirstRrquestLength)]
    public virtual string FirstRrquest { get; set; }

    [MaxLength(AgentInstanceConsts.MaxLastResponseLength)]
    public virtual string LastResponse { get; set; }

    public virtual string Steps { get; set; } = string.Empty;

    public virtual string Context { get; set; } = string.Empty;

    public virtual List<AgentInstanceRequest> Requests { get; set; }
}
public class AgentInstanceRequest : AuditedEntity<Guid>
{
    protected AgentInstanceRequest() { }
    internal AgentInstanceRequest(Guid id) : base(id) { }

    [Required]
    public virtual Guid AgentId { get; set; }

    [Required]
    public virtual Guid AgentInstanceId { get; set; }

    [MaxLength(AgentInstanceConsts.MaxFirstRrquestLength)]
    public virtual string RrquestDody { get; set; }

    [MaxLength(AgentInstanceConsts.MaxLastResponseLength)]
    public virtual string ResponseBody { get; set; }
}