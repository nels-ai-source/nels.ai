using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Nels.Aigc.Consts;
using Nels.SemanticKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json;
using Volo.Abp.Application.Dtos;

namespace Nels.Aigc.Dtos;

public class ConversationDto : AuditedEntityDto<Guid>, ISpaceIdentifier, IConversation
{
    [Required]
    public virtual Guid SpaceId { get; set; }

    [Required]
    [StringLength(AgentConversationConsts.MaxTitleLength)]
    public virtual string Title { get; set; }

    public virtual string Metadata { get; set; } = string.Empty;

    public void SetTitle(string title)
    {
        Title = title.Length > AgentConversationConsts.MaxTitleLength ? title[..AgentConversationConsts.MaxTitleLength] : title;
    }
}
public class ChatDto : AuditedEntityDto<Guid>, ISpaceIdentifier, IChat
{
    [Required]
    public virtual Guid SpaceId { get; set; }

    [Required]
    public virtual Guid ConversationId { get; set; }

    [StringLength(AgentConversationConsts.MaxQuestionLength)]
    public virtual string Question { get; set; }

    [StringLength(AgentConversationConsts.MaxAnswerLength)]
    public virtual string Answer { get; set; }

    public virtual List<IChatMessage> Messages { get; set; } = [];

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

    public IConversation Conversation { get; set; }
    public virtual void AddMessage(Guid id, ChatMessageContent content, bool insertFirst = false)
    {
        AddMessage(id: id, role: content.Role, content: content.Content ?? string.Empty, metadata: JsonSerializer.Serialize(content), insertFirst: insertFirst);
    }
    public virtual void AddMessage(Guid id, AuthorRole role, string content, string? metadata = null, bool insertFirst = false)
    {
        ChatMessageDto message = new(id, SpaceId, ConversationId, Id)
        {
            Role = role.Label,
            Content = content,
            Index = Messages.Count,
            Metadata = metadata ?? string.Empty
        };
        if (insertFirst)
        {
            Messages.Insert(0, message);
        }
        else
        {
            Messages.Add(message);
        }
        SetQuestionAndAnswer();
    }

    private void SetQuestionAndAnswer()
    {
        var userMessage = Messages.FirstOrDefault(x => x.Role == AuthorRole.User.Label)?.Content ?? string.Empty;
        var assistantMessage = Messages.LastOrDefault(x => x.Role == AuthorRole.Assistant.Label)?.Content ?? string.Empty;

        Question = userMessage.Length > AgentConversationConsts.MaxQuestionLength ? userMessage[..AgentConversationConsts.MaxQuestionLength] : userMessage;
        Answer = assistantMessage.Length > AgentConversationConsts.MaxAnswerLength ? assistantMessage[..AgentConversationConsts.MaxAnswerLength] : assistantMessage;

        if (string.IsNullOrWhiteSpace(Conversation.Title))
        {
            Conversation.SetTitle(Question);
        }
    }
}

public class ChatMessageDto : EntityDto<Guid>, ISpaceIdentifier, IChatMessage
{
    public ChatMessageDto() { }
    internal ChatMessageDto(Guid id, Guid spaceId, Guid conversationId, Guid chatId)
    {
        Id = id;
        SpaceId = spaceId;
        ConversationId = conversationId;
        ChatId = chatId;
        Role = AuthorRole.Assistant.Label;
    }
    [Required]
    public virtual Guid SpaceId { get; set; }

    [Required]
    public virtual Guid ConversationId { get; set; }

    [Required]
    public virtual Guid ChatId { get; set; }

    [Required]
    [MaxLength(AgentMessageConsts.MaxRoleLength)]
    public virtual string Role { get; set; } = AuthorRole.Assistant.Label;

    public virtual string Content { get; set; }

    public virtual string Metadata { get; set; }
    public virtual int Index { get; set; }
}
