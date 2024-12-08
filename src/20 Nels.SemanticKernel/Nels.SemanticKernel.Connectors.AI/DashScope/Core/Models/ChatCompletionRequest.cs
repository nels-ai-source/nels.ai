using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace Nels.SemanticKernel.DashScope.Core.Models;

/// <summary>
/// DashScope text generation request object.
/// </summary>
internal sealed class ChatCompletionRequest
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
    public ChatInput Input { get; set; }

    [JsonPropertyName("parameters")]
    public ChatParameter? Parameters { get; set; }

    /// <summary>
    /// Converts a <see cref="PromptExecutionSettings" /> object to a <see cref="TextGenerationRequest" /> object.
    /// </summary>
    /// <param name="chatHistory">Chat history to be used for the request.</param>
    /// <param name="executionSettings">Execution settings to be used for the request.</param>
    /// <returns>TexGenerationtRequest object.</returns>
    internal static ChatCompletionRequest FromChatHistoryAndExecutionSettings(ChatHistory chatHistory, DashScopePromptExecutionSettings executionSettings, string? deploymentName)
    {
        return new ChatCompletionRequest
        {
            Input = new ChatInput
            {
                Messages = chatHistory.Select(message => new ChatMessage
                {
                    Content = message.Content,
                    Role = message.Role.ToString(),
                }).ToList()
            },
            Model = deploymentName ?? (executionSettings.ModelId ?? TextGenerationInferenceDefaultModel),
            Parameters = new ChatParameter
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

    internal sealed class ChatMessageToolCall
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("type")]
        public string? Type { get; set; }

        [JsonPropertyName("function")]
        public ChatMessageFunction? Function { get; set; }
    }

    internal sealed class ChatMessageFunction
    {
        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("parameters")]
        public string? Parameters { get; set; }
    }

    internal sealed class ChatInput
    {
        [JsonPropertyName("messages")]
        public List<ChatMessage>? Messages { get; set; }
    }

    internal sealed class ChatMessage
    {
        [JsonPropertyName("role")]
        public string? Role { get; set; }

        [JsonPropertyName("content")]
        public string? Content { get; set; }

        [JsonPropertyName("name")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Name { get; set; }

        [JsonPropertyName("tool_calls")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<ChatMessageToolCall>? ToolCalls { get; set; }
    }

    internal sealed class ChatParameter
    {
        /// <summary>
        /// 用于指定返回结果的格式，默认为text，也可设置为message。当设置为message时，输出格式请参考返回结果。推荐优先使用message格式。
        /// </summary>
        [JsonPropertyName("result_format")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? ResultFormat { get; set; }
        /// <summary>
        /// 生成时使用的随机数种子，用户控制模型生成内容的随机性。seed支持无符号64位整数。在使用seed时，模型将尽可能生成相同或相似的结果，但目前不保证每次生成的结果完全相同。
        /// </summary>
        [JsonPropertyName("seed")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public long? Seed { get; set; }
        /// <summary>
        /// 用于限制模型生成token的数量，表示生成token个数的上限。其中qwen-turbo最大值和默认值为1500，qwen-max、qwen-max-1201 、qwen-max-longcontext 和 qwen-plus最大值和默认值均为2000。
        /// </summary>
        [JsonPropertyName("max_tokens")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? MaxTokens { get; set; }
        /// <summary>
        /// 生成时，核采样方法的概率阈值。例如，取值为0.8时，仅保留累计概率之和大于等于0.8的概率分布中的token，作为随机采样的候选集。取值范围为（0,1.0)，取值越大，生成的随机性越高；取值越低，生成的随机性越低。注意，取值不要大于等于1。
        /// </summary>
        [JsonPropertyName("top_p")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public float? TopP { get; set; }
        /// <summary>
        /// 生成时，采样候选集的大小。例如，取值为50时，仅将单次生成中得分最高的50个token组成随机采样的候选集。取值越大，生成的随机性越高；取值越小，生成的确定性越高。注意：如果top_k参数为空或者top_k的值大于100，表示不启用top_k策略，此时仅有top_p策略生效。
        /// </summary>
        [JsonPropertyName("top_k")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? Topk { get; set; }
        /// <summary>
        /// 用于控制模型生成时连续序列中的重复度。提高repetition_penalty时可以降低模型生成的重复度。1.0表示不做惩罚。没有严格的取值范围。
        /// </summary>
        [JsonPropertyName("repetition_penalty")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public float? RepetitionPenalty { get; set; }
        /// <summary>
        /// 用户控制模型生成时整个序列中的重复度。提高presence_penalty时可以降低模型生成的重复度，取值范围 [-2.0, 2.0]。
        /// </summary>
        [JsonPropertyName("presence_penalty")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public float? PresencePenalty { get; set; }
        /// <summary>
        /// 用于控制随机性和多样性的程度。具体来说，temperature值控制了生成文本时对每个候选词的概率分布进行平滑的程度。较高的temperature值会降低概率分布的峰值，使得更多的低概率词被选择，生成结果更加多样化；而较低的temperature值则会增强概率分布的峰值，使得高概率词更容易被选择，生成结果更加确定。取值范围：[0, 2)，不建议取值为0，无意义。
        /// </summary>
        [JsonPropertyName("temperature")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public float? Temperature { get; set; }
        /// <summary>
        /// stop参数用于实现内容生成过程的精确控制，在模型生成的内容即将包含指定的字符串或token_id时自动停止，生成的内容不包含指定的内容。stop可以为string类型或array类型。
        /// </summary>
        [JsonPropertyName("stop")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<string>? Stop { get; set; }
        /// <summary>
        /// 模型内置了互联网搜索服务，该参数控制模型在生成文本时是否参考使用互联网搜索结果。
        /// </summary>
        [JsonPropertyName("enable_search")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? EnableSearch { get; set; }
        /// <summary>
        /// 控制在流式输出模式下是否开启增量输出，即后续输出内容是否包含已输出的内容。设置为True时，将开启增量输出模式，后面输出不会包含已经输出的内容，您需要自行拼接整体输出；设置为False则会包含已输出的内容。
        /// </summary>
        [JsonPropertyName("incremental_output")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? IncrementalOutput { get; set; }
    }
}
