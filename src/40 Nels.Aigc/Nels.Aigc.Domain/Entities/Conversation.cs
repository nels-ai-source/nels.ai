using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Nels.Aigc.Consts;
using Nels.SemanticKernel.Interfaces;
using Nels.SemanticKernel.Process.Consts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json;
using Volo.Abp;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;

namespace Nels.Aigc.Entities;

public class Conversation : FullAuditedEntity<Guid>, ISoftDelete, ISpaceIdentifier
{
    public Conversation() { }
    public Conversation(Guid id, Guid spaceId) : base(id)
    {
        SpaceId = spaceId;
    }

    [Required]
    public virtual Guid SpaceId { get; set; } = default!;

    [Required]
    [MaxLength(AgentConversationConsts.MaxTitleLength)]
    public virtual string Title { get; set; }

    public virtual string Metadata { get; set; } = string.Empty;

    public virtual List<Chat> Chats { get; set; }

    public virtual void SetTitle(string title)
    {
        Title = title.Length > AgentConversationConsts.MaxTitleLength ? title[..AgentConversationConsts.MaxTitleLength] : title;
    }
}
public class Chat : FullAuditedEntity<Guid>, IAggregateRoot, ISoftDelete, ISpaceIdentifier
{
    public Chat() { }
    public Chat(Guid id, Guid spaceId, Guid conversationId) : base(id)
    {
        SpaceId = spaceId;
        ConversationId = conversationId;
    }

    [Required]
    public virtual Guid SpaceId { get; set; } = default!;

    [Required]
    public virtual Guid ConversationId { get; set; }

    [MaxLength(AgentConversationConsts.MaxQuestionLength)]
    public virtual string Question { get; set; }

    [MaxLength(AgentConversationConsts.MaxAnswerLength)]
    public virtual string Answer { get; set; }

    [Required]
    public virtual double TotalResponseDuration { get; set; } = 0;

    [Required]
    public virtual double FirstTokenResponseDuration { get; set; } = 0;

    [Required]
    public virtual int InputTokenCount { get; set; } = 0;

    [Required]
    public virtual int OutputTokenCount { get; set; } = 0;

    [Required]
    public virtual int TotalTokenCount { get; set; } = 0;

    public virtual List<ChatMessage> Messages { get; set; } = [];


}
public class ChatMessage : FullAuditedEntity<Guid>, ISoftDelete, ISpaceIdentifier
{
    protected ChatMessage() { }
    internal ChatMessage(Guid id, Guid spaceId, Guid conversationId, Guid chatId) : base(id)
    {
        SpaceId = spaceId;
        ConversationId = conversationId;
        ChatId = chatId;
        Role = AuthorRole.Assistant.Label;
    }

    [Required]
    public virtual Guid SpaceId { get; set; } = default!;

    [Required]
    public virtual Guid ConversationId { get; set; }

    [Required]
    public virtual Guid ChatId { get; set; }

    [Required]
    [MaxLength(AgentMessageConsts.MaxRoleLength)]
    public virtual string Role { get; set; }

    public virtual string Content { get; set; }

    public virtual string Metadata { get; set; }

    public virtual int Index { get; set; } = 0;
}

