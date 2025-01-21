using Microsoft.AspNetCore.Http;
using Nels.SemanticKernel.Interfaces;
using Nels.SemanticKernel.Text;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace Nels.SemanticKernel.Services;

public class SseStreamResponse : IStreamResponse
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    public SseStreamResponse(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public void EnableStream()
    {
        _httpContextAccessor.HttpContext.Response.ContentType = "text/event-stream";
        _httpContextAccessor.HttpContext.Response.Headers.Append("Cache-Control", "no-cache");
    }

    public async Task WriteDataAsync(string eventType, dynamic data)
    {
        string text = JsonSerializer.Serialize(data);
        await SseWriteAsync(eventType, text);
    }

    public async Task WriteMessagAsync(Guid messageId, object content)
    {
        await WriteDataAsync(EventType.Text, new Dictionary<string, object>
            {
                { nameof(messageId),messageId},
                { nameof(content),content}
            });
    }

    private async Task SseWriteAsync(string eventType, string? text)
    {
        if (_httpContextAccessor?.HttpContext != null && string.IsNullOrEmpty(text) == false)
        {
            await _httpContextAccessor.HttpContext.Response.Body.WriteAsync(new SseData(eventType, text).ToBytes());
        }
    }
}
public class EventType
{
    public static string Text = "text";

    public static string Data = "data";

    public static string Down = "down";
}
