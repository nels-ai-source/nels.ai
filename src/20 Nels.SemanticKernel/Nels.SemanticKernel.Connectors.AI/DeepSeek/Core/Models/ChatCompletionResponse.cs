using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Nels.SemanticKernel.DeepSeek.Core.Models;
internal sealed class ChatCompletionResponse
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("created")]
    public int Created { get; set; } = default;

    [JsonPropertyName("choices")]
    public List<Choice> Choices { get; set; }

    [JsonPropertyName("usage")]
    public CompletionUsage Usage { get; set; }

    [JsonPropertyName("model")]
    public string Model { get; set; } = string.Empty;

    [JsonPropertyName("object")]
    public string Object { get; set; } = string.Empty;

    internal sealed class Choice
    {
        [JsonPropertyName("finish_reason")]
        public string FinishReason { get; set; }

        [JsonPropertyName("index")]
        public int Index { get; set; }

        [JsonPropertyName("message")]
        public Message Message { get; set; }
    }

    internal sealed class Message
    {
        [JsonPropertyName("content")]
        public string Content { get; set; }

        [JsonPropertyName("role")]
        public string Role { get; set; }
    }

    internal sealed class CompletionUsage
    {
        [JsonPropertyName("prompt_tokens")]
        public int PromptTokens { get; set; }

        [JsonPropertyName("completion_tokens")]
        public int CompletionTokens { get; set; }

        [JsonPropertyName("total_tokens")]
        public int TotalTokens { get; set; }
    }
}
