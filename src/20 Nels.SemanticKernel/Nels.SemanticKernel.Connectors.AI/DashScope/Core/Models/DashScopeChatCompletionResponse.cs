using Nels.SemanticKernel.InternalUtilities.Functions;
using System.Collections.Generic;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace Nels.SemanticKernel.DashScope.Core.Models;

internal sealed class DashScopeChatCompletionResponse
{
    [JsonPropertyName("output")]
    public DashScopeChatOutput Output { get; set; }

    [JsonPropertyName("usage")]
    public DashScopeCompletionUsage Usage { get; set; }


    internal sealed class DashScopeChatOutput
    {
        [JsonPropertyName("text")]
        public string Text { get; set; }

        [JsonPropertyName("finish_reason")]
        public string finishReason { get; set; }

        [JsonPropertyName("created")]
        public long Created { get; set; }

        [JsonPropertyName("choices")]
        public List<DashScopeChoice> Choices { get; set; }
    }

    internal sealed class DashScopeChoice
    {
        [JsonPropertyName("finish_reason")]
        public string FinishReason { get; set; }

        [JsonPropertyName("message")]
        public DashScopeMessage Message { get; set; }
    }

    internal sealed class DashScopeMessage
    {
        [JsonPropertyName("content")]
        public string Content { get; set; }

        [JsonPropertyName("tool_calls")]
        public List<DashScopeChoiceToolCall> ToolCalls { get; set; }

        [JsonPropertyName("function_call")]
        public DashScopeChoiceToolCallFunction FunctionCall { get; set; }

        [JsonPropertyName("role")]
        public string Role { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }

    internal sealed class DashScopeChoiceToolCall
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("function")]
        public DashScopeChoiceToolCallFunction Function { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("index")]
        public int Index { get; set; }
    }

    internal sealed class DashScopeChoiceToolCallFunction : IFunctionCall
    {
        [JsonPropertyName("name")]
        [JsonRequired]
        public string FunctionName { get; set; }

        [JsonPropertyName("arguments")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public JsonNode Arguments { get; set; }

        public override string ToString()
        {
            return $"FunctionName={this.FunctionName}, Arguments={this.Arguments}";
        }
    }

    internal sealed class DashScopeCompletionUsage
    {
        [JsonPropertyName("input_tokens")]
        public int InputTokens { get; set; }

        [JsonPropertyName("output_tokens")]
        public int OutputTokens { get; set; }

        [JsonPropertyName("total_tokens")]
        public int TotalTokens { get; set; }
    }
}
