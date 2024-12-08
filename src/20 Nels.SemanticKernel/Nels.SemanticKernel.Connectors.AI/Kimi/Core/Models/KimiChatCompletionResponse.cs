using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Nels.SemanticKernel.Kimi.Core.Models;

internal sealed class KimiChatCompletionResponse
{
    [JsonPropertyName("id")]
    public string Id { get; set; }


    [JsonPropertyName("object")]
    public string Object { get; set; }

    [JsonPropertyName("created")]
    public int? Created { get; set; }


    [JsonPropertyName("model")]
    public string Model { get; set; }


    [JsonPropertyName("choices")]
    public List<ChatChoices> Choices { get; set; }


    [JsonPropertyName("usage")]
    public CompletionUsage Usage { get; set; }


    internal sealed class ChatChoices
    {
        [JsonPropertyName("index")]
        public int? Index { get; set; }


        [JsonPropertyName("delta")]
        public CompletionMessage Delta { get; set; }


        [JsonPropertyName("finish_reason")]
        public string FinishReason { get; set; }
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

    internal sealed class CompletionMessage
    {
        [JsonPropertyName("role")]
        public string Role { get; set; }


        [JsonPropertyName("content")]
        public string Content { get; set; }
    }

}
