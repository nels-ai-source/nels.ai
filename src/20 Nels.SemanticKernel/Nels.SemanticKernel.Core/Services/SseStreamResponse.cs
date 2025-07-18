using Microsoft.AspNetCore.Http;
using Microsoft.SemanticKernel.ChatCompletion;
using Nels.SemanticKernel.Consts;
using Nels.SemanticKernel.Interfaces;
using Nels.SemanticKernel.Text;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace Nels.SemanticKernel.Services;

public class SseStreamResponse(IHttpContextAccessor httpContextAccessor) : IStreamResponse
{
    private Guid conversationId = Guid.Empty, chatId = Guid.Empty;
    public Task ChatCreated(Guid chatId, Guid conversationId)
    {
        EnableStream();

        this.chatId = chatId;
        this.conversationId = conversationId;

        ChatCreatedContent content = new()
        {
            ChatId = chatId,
            ConversationId = conversationId
        };

        return SseWriteAsync(ConversationEventConsts.ChatCreated, JsonSerializer.Serialize(content));
    }

    public Task ChatFailed(string code, string msg)
    {
        ChatFailedContent content = new()
        {
            ChatId = chatId,
            ConversationId = conversationId,
            Code = code,
            Msg = msg
        };

        return SseWriteAsync(ConversationEventConsts.ChatFailed, JsonSerializer.Serialize(content));
    }

    public Task ChatInProgress()
    {
        ChatInProgressContent content = new()
        {
            ChatId = chatId,
            ConversationId = conversationId
        };

        return SseWriteAsync(ConversationEventConsts.ChatFailed, JsonSerializer.Serialize(content));
    }

    public async Task Down()
    {
        await SseWriteAsync(ConversationEventConsts.Done, "[DONE]");
    }


    public Task MessageDelta(string content, string role = null, string? type = null, string? contentType = null)
    {
        MessageContent chatMessageContent = new()
        {
            ChatId = chatId,
            ConversationId = conversationId,
            Role = role ?? AuthorRole.Assistant.Label,
            Type = type ?? MessageTypeConsts.Answer,
            Content = content,
            ContentType = contentType ?? MessageContentTypeConsts.Text
        };
        return SseWriteAsync(ConversationEventConsts.MessageDelta, JsonSerializer.Serialize(chatMessageContent));
    }
    public Task MessageCompleted(string content, string? role = null, string? type = null, string? contentType = null)
    {
        MessageContent chatMessageContent = new()
        {
            ChatId = chatId,
            ConversationId = conversationId,
            Role = role ?? AuthorRole.Assistant.Label,
            Type = type ?? MessageTypeConsts.Answer,
            Content = content,
            ContentType = contentType ?? MessageContentTypeConsts.Text
        };
        return SseWriteAsync(ConversationEventConsts.MessageCompleted, JsonSerializer.Serialize(chatMessageContent));
    }

    private void EnableStream()
    {
        var response = httpContextAccessor.HttpContext.Response;

        // 设置SSE必需的响应头
        response.ContentType = "text/event-stream";
        response.Headers.Append("Cache-Control", "no-cache");
        response.Headers.Append("Connection", "keep-alive");
        response.Headers.Append("X-Accel-Buffering", "no");

        // 添加CORS支持
        response.Headers.Append("Access-Control-Allow-Origin", "*");
        response.Headers.Append("Access-Control-Allow-Headers", "Cache-Control");

        // 立即发送响应头，防止缓冲
        response.Body.FlushAsync().Wait();
    }
    private async Task SseWriteAsync(string eventType, string? text)
    {
        if (httpContextAccessor?.HttpContext != null && string.IsNullOrEmpty(text) == false)
        {
            await httpContextAccessor.HttpContext.Response.Body.WriteAsync(new SseData(eventType, text).ToBytes());
            await httpContextAccessor.HttpContext.Response.Body.FlushAsync();
        }
    }
}
