using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Nels.SemanticKernel.DashScope.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace Nels.SemanticKernel.DashScope.Core.Models;

/// <summary>
/// DashScope text generation request object.
/// </summary>
internal sealed class DashScopeChatCompletionRequest
{
    /// <summary>
    /// This is the default name when using qwen-max and will be ignored as the qwen-max will only target the current activated model.
    /// </summary>
    private const string TextGenerationInferenceDefaultModel = "qwen-max";
    /// <summary>
    /// Model name to use for generation.
    /// </summary>
    /// <remarks>
    /// When using TGI this parameter will be ignored.
    /// </remarks>
    [JsonPropertyName("model")]
    public string? Model { get; set; }

    [JsonPropertyName("input")]
    public DashScopeChatInput Input { get; set; }

    [JsonPropertyName("parameters")]
    public DashScopeChatParameter? Parameters { get; set; }

    [JsonPropertyName("tool_choice")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ToolChoice { get; set; }

    public void AddFunction(DashScopeFunction function)
    {
        this.Parameters.Tools ??= [];
        this.Parameters.Tools.Add(new DashScopeTool { Function = function.ToFunctionDeclaration() });
    }

    public void AddChatMessage(ChatMessageContent message)
    {
        Verify.NotNull(this.Input?.Messages);
        Verify.NotNull(message);

        this.Input?.Messages.Add(CreateContentFromChatMessage(message));
    }
    private static DashScopeChatMessage CreateContentFromChatMessage(ChatMessageContent message)
    {
        return new DashScopeChatMessage
        {
            Role = message.Role.Label,
            Content = message.Content,
        };
    }

    /// <summary>
    /// Converts a <see cref="PromptExecutionSettings" /> object to a <see cref="TextGenerationRequest" /> object.
    /// </summary>
    /// <param name="chatHistory">Chat history to be used for the request.</param>
    /// <param name="executionSettings">Execution settings to be used for the request.</param>
    /// <returns>TexGenerationtRequest object.</returns>
    internal static DashScopeChatCompletionRequest FromChatHistoryAndExecutionSettings(ChatHistory chatHistory, DashScopePromptExecutionSettings executionSettings, string? deploymentName)
    {
        return new DashScopeChatCompletionRequest
        {
            Input = new DashScopeChatInput
            {
                Messages = chatHistory.Select(message => new DashScopeChatMessage
                {
                    Content = message.Content,
                    Role = message.Role.ToString(),
                }).ToList()
            },
            Model = deploymentName ?? (executionSettings.ModelId ?? TextGenerationInferenceDefaultModel),
            Parameters = new DashScopeChatParameter
            {
                ResultFormat = "message",
                Seed = executionSettings.Seed,
                MaxTokens = executionSettings.MaxTokens,
                TopP = executionSettings.TopP,
                Topk = null,
                RepetitionPenalty = executionSettings.RepetitionPenalty,
                PresencePenalty = executionSettings.PresencePenalty,
                Temperature = executionSettings.Temperature,
                Stop = executionSettings.Stop,
                EnableSearch = false,
                IncrementalOutput = true
            }
        };
    }

    internal sealed class DashScopeChatInput
    {
        [JsonPropertyName("messages")]
        public List<DashScopeChatMessage>? Messages { get; set; }
    }

    internal sealed class DashScopeChatMessage
    {
        [JsonPropertyName("role")]
        public string? Role { get; set; }

        [JsonPropertyName("content")]
        public string? Content { get; set; }
    }

    internal sealed class DashScopeChatParameter
    {

        [JsonPropertyName("result_format")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? ResultFormat { get; set; }

        [JsonPropertyName("seed")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public long? Seed { get; set; }

        [JsonPropertyName("max_tokens")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? MaxTokens { get; set; }

        [JsonPropertyName("top_p")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public float? TopP { get; set; }

        [JsonPropertyName("top_k")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? Topk { get; set; }

        [JsonPropertyName("repetition_penalty")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public float? RepetitionPenalty { get; set; }

        [JsonPropertyName("presence_penalty")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public float? PresencePenalty { get; set; }

        [JsonPropertyName("temperature")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public float? Temperature { get; set; }

        [JsonPropertyName("stop")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<string>? Stop { get; set; }

        [JsonPropertyName("enable_search")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? EnableSearch { get; set; }

        [JsonPropertyName("incremental_output")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? IncrementalOutput { get; set; }

        [JsonPropertyName("tools")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IList<DashScopeTool>? Tools { get; set; }
    }

    internal sealed class DashScopeTool
    {
        [JsonPropertyName("type")]
        public string Type { get; set; } = "function";

        /// <summary>
        /// A list of FunctionDeclarations available to the model that can be used for function calling.
        /// </summary>
        /// <remarks>
        /// The model or system does not execute the function. Instead the defined function may be returned as a
        /// [FunctionCall][content.part.function_call] with arguments to the client side for execution.
        /// The model may decide to call a subset of these functions by populating
        /// [FunctionCall][content.part.function_call] in the response. The next conversation turn may contain
        /// a [FunctionResponse][content.part.function_response] with the [content.role] "function" generation context for the next model turn.
        /// </remarks>
        [JsonPropertyName("functionDeclarations")]
        public FunctionDeclaration Function { get; set; }

        /// <summary>
        /// Structured representation of a function declaration as defined by the OpenAPI 3.03 specification.
        /// Included in this declaration are the function name and parameters.
        /// This FunctionDeclaration is a representation of a block of code that can be used as a Tool by the model and executed by the client.
        /// </summary>
  
    }
    internal sealed class FunctionDeclaration
    {
        /// <summary>
        /// Required. Name of function.
        /// </summary>
        /// <remarks>
        /// Must be a-z, A-Z, 0-9, or contain underscores and dashes, with a maximum length of 63.
        /// </remarks>
        [JsonPropertyName("name")]
        public string Name { get; set; } = null!;

        /// <summary>
        /// Required. A brief description of the function.
        /// </summary>
        [JsonPropertyName("description")]
        public string Description { get; set; } = null!;

        /// <summary>
        /// Optional. Describes the parameters to this function.
        /// Reflects the Open API 3.03 Parameter Object string Key: the name of the parameter.
        /// Parameter names are case sensitive. Schema Value: the Schema defining the type used for the parameter.
        /// </summary>
        [JsonPropertyName("parameters")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public JsonNode Parameters { get; set; }
    }
}
