using Nels.Aigc.Consts;
using Nels.Aigc.Enums;
using Nels.SemanticKernel.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
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

    public virtual AgentKnowledgeOption KnowledgeOption { get; set; }

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

    public void SyncPresetQuestions(ICollection<AgentPresetQuestions> newQuestions)
    {
        var questionsToAdd = newQuestions.Where(nq => !Questions.Any(q => q.Content == nq.Content)).ToList();
        var questionsToRemove = Questions.Where(q => !newQuestions.Any(nq => nq.Content == q.Content)).ToList();

        foreach (var q in questionsToAdd)
        {
            AddPresetQuestion(Guid.NewGuid(), q.Content);
        }

        foreach (var q in questionsToRemove)
        {
            RemovePresetQuestion(q.Id);
        }
    }

    public void SyncKnowledges(ICollection<AgentKnowledge> newKnowledges)
    {
        var knowledgesToAdd = newKnowledges.Where(nk => !Knowledges.Any(k => k.Id == nk.Id)).ToList();
        var knowledgesToRemove = Knowledges.Where(k => !newKnowledges.Any(nk => nk.Id == k.Id)).ToList();

        foreach (var k in knowledgesToAdd)
        {
            AddKnowledge(k.Id, k.KnowledgeId);
        }

        foreach (var k in knowledgesToRemove)
        {
            RemoveKnowledge(k.KnowledgeId);
        }
    }

    public void SyncTools(ICollection<AgentTool> newTools)
    {
        var toolsToAdd = newTools.Where(nt => !Tools.Any(t => t.ToolId == nt.ToolId)).ToList();
        var toolsToRemove = Tools.Where(t => !newTools.Any(nt => nt.ToolId == t.ToolId)).ToList();

        foreach (var t in toolsToAdd)
        {
            AddTool(Guid.NewGuid(), t.ToolId);
        }

        foreach (var t in toolsToRemove)
        {
            RemoveTool(t.ToolId);
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
public class AgentKnowledgeOption : Entity<Guid>
{
    protected AgentKnowledgeOption() { }
    internal AgentKnowledgeOption(Guid id, Guid agentId) : base(id)
    {
        AgentId = agentId;
    }
    public virtual Guid AgentId { get; set; }
    public virtual bool AutoInvoke { get; set; } = true;
    public virtual SearchStrategy SearchStrategy { get; set; } = SearchStrategy.Hybrid;
    public virtual int MaxRecallCount { get; set; } = 3;
    public virtual double MinMatchScore { get; set; } = 0.5;
    public virtual ReplyMode ReplyMode { get; set; } = ReplyMode.Default;
    [MaxLength(AgentKnowledgeOptionConsts.MaxCustomReplyLength)]
    public virtual string CustomReply { get; set; } = string.Empty;
    public virtual bool ShowSource { get; set; } = true;
    public virtual SourceDisplayMode SourceDisplayMode { get; set; } = SourceDisplayMode.Card;
}