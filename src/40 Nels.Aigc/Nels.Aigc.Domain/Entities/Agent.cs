using Nels.Aigc.Consts;
using Nels.Aigc.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.Guids;

namespace Nels.Aigc.Entities;

public class Agent : AuditedEntity<Guid>, IAggregateRoot<Guid>
{
    public Agent() { }
    public Agent(Guid id) : base(id) { }

    [Required]
    public virtual Guid SpaceId { get; set; } = Guid.Empty;

    [Required]
    [MaxLength(PromptConsts.MaxNameLength)]
    public virtual string Name { get; set; } = default!;

    [MaxLength(PromptConsts.MaxDescriptionLength)]
    public virtual string Description { get; set; } = default!;
    public virtual AgentType AgentType { get; set; } = AgentType.Llm;
    public virtual string IntroductionText { get; set; } = string.Empty;
    public virtual List<AgentPresetQuestions> PresetQuestions { get; set; } = [];
    public virtual AgentMetadata? Metadata { get; set; }

    public void AddOrUpdateMetadata(Guid id, string metadata)
    {
        Metadata ??= new AgentMetadata(id, this.Id, metadata);
        Metadata.Metadata = metadata;
    }
}
public class AgentPresetQuestions : AuditedEntity<Guid>
{
    protected AgentPresetQuestions() { }
    internal AgentPresetQuestions(Guid id, Guid agentId, string content) : base(id)
    {
        AgentId = agentId;
        Content = content;
    }
    public virtual Guid AgentId { get; set; }
    public virtual string Content { get; set; }

    public virtual int Index { get; set; }
}
public class AgentMetadata : AuditedEntity<Guid>
{
    protected AgentMetadata() { }
    internal AgentMetadata(Guid id, Guid agentId, string metadata) : base(id)
    {
        AgentId = agentId;
        Metadata = metadata;
    }
    public virtual Guid AgentId { get; set; }
    public virtual string Metadata { get; set; } = string.Empty;
}
