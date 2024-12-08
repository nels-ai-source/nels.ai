using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Nels.SemanticKernel.DashScope.Core.Models;
internal sealed class ChatCompletionResponse
{
    [JsonPropertyName("output")]
    public ChatOutput Output { get; set; }

    [JsonPropertyName("usage")]
    public CompletionUsage Usage { get; set; }
    internal sealed class ChatOutput
    {
        [JsonPropertyName("text")]
        public string Text { get; set; }

        [JsonPropertyName("finish_reason")]
        public string finishReason { get; set; }

        [JsonPropertyName("created")]
        public long Created { get; set; }

        [JsonPropertyName("choices")]
        public List<Choice> Choices { get; set; }
    }

    internal sealed class Choice
    {
        [JsonPropertyName("finish_reason")]
        public string FinishReason { get; set; }

        [JsonPropertyName("message")]
        public Message Message { get; set; }
    }

    internal sealed class Message
    {
        [JsonPropertyName("content")]
        public string Content { get; set; }

        [JsonPropertyName("tool_calls")]
        public List<ChoiceToolCall> ToolCalls { get; set; }

        [JsonPropertyName("function_call")]
        public ChoiceToolCallFunction FunctionCall { get; set; }

        [JsonPropertyName("role")]
        public string Role { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }

    internal sealed class ChoiceToolCall
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("function")]
        public ChoiceToolCallFunction Function { get; set; }
    }

    internal sealed class ChoiceToolCallFunction
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("arguments")]
        public string Arguments { get; set; }
    }

    internal sealed class CompletionUsage
    {
        [JsonPropertyName("input_tokens")]
        public int InputTokens { get; set; }

        [JsonPropertyName("output_tokens")]
        public int OutputTokens { get; set; }

        [JsonPropertyName("total_tokens")]
        public int TotalTokens { get; set; }
    }
}
