using Nels.Aigc.Consts;
using Nels.SemanticKernel.Process.Consts;
using Nels.SemanticKernel.Process.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;

namespace Nels.Aigc.Entities;

public class AgentConversation : AuditedEntity<Guid>, IAggregateRoot<Guid>
{
    public AgentConversation() { }
    public AgentConversation(Guid id) : base(id) { }

    [Required]
    public virtual Guid AgentId { get; set; }

    [Required]
    [MaxLength(AgentConversationConsts.MaxTitleLength)]
    public virtual string Title { get; set; }

    public virtual string Metadata { get; set; } = string.Empty;

    public virtual List<AgentChat> Chats { get; set; }
}
public class AgentChat : AuditedEntity<Guid>, IAgentChat
{
    public AgentChat() { }
    public AgentChat(Guid id, Guid agentId, Guid agentConversationId) : base(id)
    {
        AgentId = agentId;
        AgentConversationId = agentConversationId;
    }

    [Required]
    public virtual Guid AgentId { get; set; }

    [Required]
    public virtual Guid AgentConversationId { get; set; }

    [MaxLength(AgentConversationConsts.MaxQuestionLength)]
    public virtual string Question { get; set; }

    [MaxLength(AgentConversationConsts.MaxAnswerLength)]
    public virtual string Answer { get; set; }

    public virtual List<AgentStepLog> StepLogs { get; set; } = [];

    public virtual List<AgentMessage> Messages { get; set; } = [];

    public virtual IStepLog AddStepLog(Guid stepId)
    {
        AgentStepLog agentStepLog = new(Guid.NewGuid())
        {
            AgentId = AgentId,
            AgentConversationId = AgentConversationId,
            AgentChatId = Id,
            StepId = stepId,
            Index = StepLogs.Count,
        };
        StepLogs.Add(agentStepLog);
        return agentStepLog;
    }

    public virtual void AddMessage(string role, string content, string type = MessageTypeConsts.Answer, string contentType = MessageContentTypeConsts.Text, string? metadata = null)
    {
        AgentMessage message = new(Guid.NewGuid())
        {
            AgentId = AgentId,
            AgentConversationId = AgentConversationId,
            Role = role,
            Content = content,
            Type = type,
            ContentType = contentType,
            Metadata = metadata ?? string.Empty
        };
        Messages.Add(message);
        SetQuestionAndAnswer();
    }

    public virtual void SetQuestionAndAnswer()
    {
        var userMessage = Messages.FirstOrDefault(x => x.Role == MessageRoleConsts.User)?.Content ?? string.Empty;
        var assistantMessage = Messages.LastOrDefault(x => x.Role == MessageRoleConsts.Assistant)?.Content ?? string.Empty;

        Question = userMessage.Length > AgentConversationConsts.MaxQuestionLength ? userMessage.Substring(0, AgentConversationConsts.MaxQuestionLength) : userMessage;
        Answer = assistantMessage.Length > AgentConversationConsts.MaxAnswerLength ? assistantMessage.Substring(0, AgentConversationConsts.MaxAnswerLength) : assistantMessage;
    }
}

public class AgentMessage : AuditedEntity<Guid>
{
    internal AgentMessage() { }
    internal AgentMessage(Guid id) : base(id) { }

    [Required]
    public virtual Guid AgentId { get; set; }

    [Required]
    public virtual Guid AgentConversationId { get; set; }

    [Required]
    [MaxLength(AgentMessageConsts.MaxRoleLength)]
    public virtual string Role { get; set; }

    [MaxLength(AgentMessageConsts.MaxTypeLength)]
    public virtual string Type { get; set; }

    [MaxLength(AgentMessageConsts.MaxContentTypeLength)]
    public virtual string ContentType { get; set; }

    public virtual string Content { get; set; }

    public virtual string Metadata { get; set; }
}

public class AgentStepLog : AuditedEntity<Guid>, IStepLog
{
    internal AgentStepLog() { }
    internal AgentStepLog(Guid id) : base(id) { }

    [Required]
    public virtual Guid AgentId { get; set; }

    [Required]
    public virtual Guid AgentConversationId { get; set; }

    [Required]
    public virtual Guid AgentChatId { get; set; }

    public int Index { get; set; }
    public Guid StepId { get; set; }
    public double Duration { get; set; } = 0.000;
    public int PromptTokens { get; set; } = 0;
    public int CompleteTokens { get; set; } = 0;
    public int Tokens => PromptTokens + CompleteTokens;

    public void SetDuration(Double millisecond)
    {
        this.Duration = Math.Round(millisecond / 1000.0, 3);
    }
}
