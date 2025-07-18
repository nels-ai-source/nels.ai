using Microsoft.SemanticKernel.ChatCompletion;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nels.SemanticKernel.Interfaces;

public interface IChatService
{
    Task<IChat> NewChatAsync(Guid? conversationId, Guid spaceId);
    Task<Guid> NewMessageId();
    Task SaveChatMessageAsync(IChat chat);
    Task<IList<IChatMessage>> GetChatMessagesAsync(Guid conversationId);
}

public interface IConversation
{
    Guid Id { get; set; }
    Guid SpaceId { get; set; }
    string Title { get; set; }
    string Metadata { get; set; }
    void SetTitle(string title);
}

public interface IChat
{
    Guid Id { get; set; }
    Guid SpaceId { get; set; }
    Guid ConversationId { get; set; }
    string Question { get; set; }
    string Answer { get; set; }
    double TotalResponseDuration { get; set; }
    double FirstTokenResponseDuration { get; set; }
    int InputTokenCount { get; set; }
    int OutputTokenCount { get; set; }
    int TotalTokenCount { get; set; }
    IConversation Conversation { get; set; }
    List<IChatMessage> Messages { get; set; }
    void AddMessage(Guid id, Microsoft.SemanticKernel.ChatMessageContent content, bool insertFirst = false);
    void AddMessage(Guid id, AuthorRole role, string content, string? metadata = null, bool insertFirst = false);
}

public interface IChatMessage
{
    Guid Id { get; set; }
    Guid SpaceId { get; set; }
    Guid ConversationId { get; set; }
    Guid ChatId { get; set; }
    string Role { get; set; }
    string Content { get; set; }
    string Metadata { get; set; }
    int Index { get; set; }
}