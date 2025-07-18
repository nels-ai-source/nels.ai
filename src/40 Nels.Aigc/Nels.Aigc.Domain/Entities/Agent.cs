using Nels.Aigc.Consts;
using Nels.Aigc.Enums;
using Nels.SemanticKernel.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;

namespace Nels.Aigc.Entities;

public class Agent : AuditedEntity<Guid>, IAggregateRoot<Guid>, ISpaceIdentifier
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

    [Required]
    public virtual AgentType Type { get; set; } = AgentType.ChatCompletion;

    public virtual string Instructions { get; set; } = default!;


    [MaxLength(AgentConsts.MaxPrologueLength)]
    public virtual string Prologue { get; set; } = string.Empty;

    public virtual List<AgentPresetQuestions> Questions { get; set; } = [];

    public virtual List<AgentKnowledge> Knowledges { get; set; } = [];

    public virtual List<AgentTool> Tools { get; set; } = [];

    public void AddPresetQuestion(Guid id, string content)
    {
        if (Questions.Exists(x => x.Content == content))
        {
            return;
        }
        Questions.Add(new AgentPresetQuestions(id, this.Id, content));
    }
    public void RemovePresetQuestion(Guid questionId)
    {
        var question = Questions.Find(x => x.Id == questionId);
        if (question is not null)
        {
            Questions.Remove(question);
        }
    }

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
