using Nels.Aigc.Consts;
using Nels.Aigc.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;

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

    public void AddOrUpdateMetadata(Guid id, string steps, string states)
    {
        Metadata ??= new AgentMetadata(id, this.Id, steps, states);
        Metadata.Steps = steps;
        Metadata.States = states;
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
    internal AgentMetadata(Guid id, Guid agentId, string steps, string states) : base(id)
    {
        AgentId = agentId;
        Steps = steps;
        States = states;
    }
    public virtual Guid AgentId { get; set; }
    public virtual string Steps { get; set; } = string.Empty;
    public virtual string States { get; set; } = string.Empty;
}
