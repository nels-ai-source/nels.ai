using Microsoft.SemanticKernel.ChatCompletion;
using Nels.SemanticKernel.Consts;
using OpenAI.Chat;
using System;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Nels.SemanticKernel.Interfaces;

public interface IStreamResponse
{
    Task ChatCreated(Guid chatId, Guid conversationId);
    Task ChatInProgress();
    Task MessageDelta(string content, string role = null, string? type = null, string? contentType = null);
    Task MessageCompleted(string content, string? role = null, string? type = null, string? contentType = null);
    Task ChatFailed(string code, string msg);
    Task Down();
}

public class ChatCreatedContent
{
    [JsonPropertyName("chatId")]
    public virtual Guid ChatId { get; set; }

    [JsonPropertyName("conversationId")]
    public virtual Guid ConversationId { get; set; }
}
public class ChatInProgressContent : ChatCreatedContent
{
}

public class MessageContent : ChatCreatedContent
{
    [JsonPropertyName("role")]
    public virtual string Role { get; set; } = AuthorRole.Assistant.Label;

    [JsonPropertyName("type")]
    public virtual string Type { get; set; } = MessageTypeConsts.Answer;

    [JsonPropertyName("content")]
    public virtual string Content { get; set; } = string.Empty;

    [JsonPropertyName("contentType")]
    public virtual string ContentType { get; set; } = MessageContentTypeConsts.Text;
}

public class ChatFailedContent : ChatCreatedContent
{
    [JsonPropertyName("code")]
    public virtual string Code { get; set; } = string.Empty;

    [JsonPropertyName("msg")]
    public virtual string Msg { get; set; } = string.Empty;
}