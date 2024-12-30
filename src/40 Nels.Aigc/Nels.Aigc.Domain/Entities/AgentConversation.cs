using Nels.Aigc.Consts;
using Nels.SemanticKernel.Process.Consts;
using Nels.SemanticKernel.Process.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Volo.Abp;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;

namespace Nels.Aigc.Entities;

public class AgentConversation : FullAuditedEntity<Guid>, IAggregateRoot<Guid>, ISoftDelete
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

    public virtual void SetTitle(string title)
    {
        Title = title.Length > AgentConversationConsts.MaxTitleLength ? title[..AgentConversationConsts.MaxTitleLength] : title;
    }
}
public class AgentChat : FullAuditedEntity<Guid>, IAgentChat, ISoftDelete
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

    [Required]
    public virtual double Duration => StepLogs.Sum(x => x.Duration);

    [Required]
    public virtual int PromptTokens => StepLogs.Sum(x => x.PromptTokens);

    [Required]
    public virtual int CompleteTokens => StepLogs.Sum(x => x.CompleteTokens);

    [Required]
    public virtual int Tokens => StepLogs.Sum(x => x.Tokens);

    public virtual List<AgentStepLog> StepLogs { get; set; } = [];

    public virtual List<AgentMessage> Messages { get; set; } = [];

    public virtual IStepLog AddStepLog(Guid id, Guid stepId)
    {
        AgentStepLog agentStepLog = new(id)
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

    public virtual void AddMessage(Guid id, string role, string content, string type = MessageTypeConsts.Answer, string contentType = MessageContentTypeConsts.Text, string? metadata = null)
    {
        AgentMessage message = new(id)
        {
            AgentId = AgentId,
            AgentConversationId = AgentConversationId,
            AgentChatId = Id,
            Role = role,
            Content = content,
            Type = type,
            ContentType = contentType,
            Index = Messages.Count,
            Metadata = metadata ?? string.Empty
        };
        Messages.Add(message);
        SetQuestionAndAnswer();
    }

    public virtual void SetQuestionAndAnswer()
    {
        var userMessage = Messages.FirstOrDefault(x => x.Role == MessageRoleConsts.User)?.Content ?? string.Empty;
        var assistantMessage = Messages.LastOrDefault(x => x.Role == MessageRoleConsts.Assistant)?.Content ?? string.Empty;

        Question = userMessage.Length > AgentConversationConsts.MaxQuestionLength ? userMessage[..AgentConversationConsts.MaxQuestionLength] : userMessage;
        Answer = assistantMessage.Length > AgentConversationConsts.MaxAnswerLength ? assistantMessage[..AgentConversationConsts.MaxAnswerLength] : assistantMessage;
    }
}

public class AgentMessage : FullAuditedEntity<Guid>, ISoftDelete
{
    protected AgentMessage() { }
    internal AgentMessage(Guid id) : base(id) { }

    [Required]
    public virtual Guid AgentId { get; set; }

    [Required]
    public virtual Guid AgentConversationId { get; set; }

    [Required]
    public virtual Guid AgentChatId { get; set; }

    [Required]
    [MaxLength(AgentMessageConsts.MaxRoleLength)]
    public virtual string Role { get; set; }

    [MaxLength(AgentMessageConsts.MaxTypeLength)]
    public virtual string Type { get; set; }

    [MaxLength(AgentMessageConsts.MaxContentTypeLength)]
    public virtual string ContentType { get; set; }

    public virtual int Index { get; set; }

    public virtual string Content { get; set; }

    public virtual string Metadata { get; set; }
}

public class AgentStepLog : FullAuditedEntity<Guid>, IStepLog, ISoftDelete
{
    protected AgentStepLog() { }
    internal AgentStepLog(Guid id) : base(id) { }

    [Required]
    public virtual Guid AgentId { get; set; }

    [Required]
    public virtual Guid AgentConversationId { get; set; }

    [Required]
    public virtual Guid AgentChatId { get; set; }

    [MaxLength(AgentStepLogConsts.MaxModelIdLength)]
    public virtual string ModelId { get; set; } = string.Empty;

    public virtual int Index { get; set; }
    public virtual Guid StepId { get; set; }
    public virtual double Duration { get; set; } = 0.000;
    public virtual int PromptTokens { get; set; } = 0;
    public virtual int CompleteTokens { get; set; } = 0;
    public virtual int Tokens => PromptTokens + CompleteTokens;

    public void SetDuration(Double millisecond)
    {
        this.Duration = Math.Round(millisecond / 1000.0, 3);
    }
}
