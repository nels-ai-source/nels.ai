using Nels.Aigc.Consts;
using Nels.Aigc.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;

namespace Nels.Aigc.Entities;

public class Agent : AuditedEntity<Guid>, IAggregateRoot<Guid>
{
    public Agent() { }
    public Agent(Guid id) : base(id) { }

    [Required]
    public virtual Guid SpaceId { get; set; } = default!;

    [Required]
    [MaxLength(AgentConsts.MaxNameLength)]
    public virtual string Name { get; set; } = default!;

    [MaxLength(AgentConsts.MaxIconLength)]
    public virtual string Icon { get; set; } = default!;

    [MaxLength(AgentConsts.MaxDescriptionLength)]
    public virtual string Description { get; set; } = default!;

    public virtual AgentType Type { get; set; } = AgentType.ChatCompletion;

    public virtual string Instructions { get; set; } = default!;


    [MaxLength(AgentConsts.MaxPrologueLength)]
    public virtual string Prologue { get; set; } = string.Empty;

    public virtual List<AgentPresetQuestions> Questions { get; set; } = [];

    public virtual List<AgentKnowledge> Knowledges { get; set; } = [];

    public virtual List<AgentTool> Tools { get; set; } = [];


    public void AddKnowledge(Guid id, Guid knowledgeId)
    {
        if (Knowledges.Exists(x => x.KnowledgeId == knowledgeId))
        {
            return;
        }
        Knowledges.Add(new AgentKnowledge(id, this.Id, knowledgeId));
    }
    public void RemoveKnowledge(Guid knowledgeId)
    {
        var knowledge = Knowledges.Find(x => x.KnowledgeId == knowledgeId);
        if (knowledge is not null)
        {
            Knowledges.Remove(knowledge);
        }
    }

    public void AddTool(Guid id, Guid toolId)
    {
        if (Tools.Exists(x => x.ToolId == toolId))
        {
            return;
        }
        Tools.Add(new AgentTool(id, this.Id, toolId));
    }
    public void RemoveTool(Guid toolId)
    {
        var tool = Tools.Find(x => x.ToolId == toolId);
        if (tool is not null)
        {
            Tools.Remove(tool);
        }
    }

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
public class AgentPresetQuestions : Entity<Guid>
{
    protected AgentPresetQuestions() { }
    internal AgentPresetQuestions(Guid id, Guid agentId, string content) : base(id)
    {
        AgentId = agentId;
        Content = content;
    }
    public virtual Guid AgentId { get; set; }

    [Required]
    [MaxLength(AgentPresetQuestionsConsts.MaxContentLength)]
    public virtual string Content { get; set; }

    public virtual int Index { get; set; }
}

public class AgentTool : Entity<Guid>
{
    protected AgentTool() { }
    internal AgentTool(Guid id, Guid agentId, Guid toolId) : base(id)
    {
        AgentId = agentId;
        ToolId = toolId;
    }
    public virtual Guid AgentId { get; set; } = default!;
    public virtual Guid ToolId { get; set; } = default!;
}
public class AgentKnowledge : Entity<Guid>
{
    protected AgentKnowledge() { }
    internal AgentKnowledge(Guid id, Guid agentId, Guid knowledgeId) : base(id)
    {
        AgentId = agentId;
        KnowledgeId = knowledgeId;
    }
    public virtual Guid AgentId { get; set; }
    public virtual Guid KnowledgeId { get; set; }
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
