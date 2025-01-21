using Nels.Aigc.Consts;
using Nels.Aigc.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;

namespace Nels.Aigc.Entities;

public class AgentEntity : AuditedEntity<Guid>, IAggregateRoot<Guid>
{
    public AgentEntity() { }
    public AgentEntity(Guid id) : base(id) { }

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

    [NotMapped]
    public virtual IAgentMetadata? Metadata { get; set; }

    public void AddOrUpdateWorkflowMetadata(Guid id, string steps, string states)
    {
        if (Metadata is WorkflowAgentMetadata metadata)
        {
            metadata ??= new WorkflowAgentMetadata(id, this.Id, steps, states);
            metadata.Steps = steps;
            metadata.States = states;
        }
    }

    public void AddOrUpdateLlmWorkflowMetadata(Guid id, string prompt, int chatReducerCount = 0, bool toolAutoInvoke = false)
    {
        if (Metadata is null)
        {
            Metadata = new LlmAgentMetadata(id, this.Id, prompt, chatReducerCount, toolAutoInvoke);
            return;
        }

        if (Metadata is LlmAgentMetadata metadata)
        {
            metadata.Prompt = prompt;
            metadata.ChatReducerCount = chatReducerCount;
            metadata.ToolAutoInvoke = toolAutoInvoke;
        }
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
public class LlmAgentMetadata : AuditedEntity<Guid>, IAgentMetadata
{
    protected LlmAgentMetadata() { }

    internal LlmAgentMetadata(Guid id, Guid agentId, string prompt, int chatReducerCount = 0, bool toolAutoInvoke = false) : base(id)
    {
        AgentId = agentId;
        Prompt = prompt;
        ChatReducerCount = chatReducerCount;
        ToolAutoInvoke = toolAutoInvoke;
    }
    public virtual Guid AgentId { get; set; }
    public virtual string Prompt { get; set; } = string.Empty;
    public virtual int ChatReducerCount { get; set; } = 0;
    public virtual bool ToolAutoInvoke { get; set; } = false;

}
public class WorkflowAgentMetadata : AuditedEntity<Guid>, IAgentMetadata
{
    protected WorkflowAgentMetadata() { }
    internal WorkflowAgentMetadata(Guid id, Guid agentId, string steps, string states) : base(id)
    {
        AgentId = agentId;
        Steps = steps;
        States = states;
    }
    public virtual Guid AgentId { get; set; }
    public virtual string Steps { get; set; } = string.Empty;
    public virtual string States { get; set; } = string.Empty;
}

public interface IAgentMetadata
{

}
