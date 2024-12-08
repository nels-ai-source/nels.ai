using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace Nels.SemanticKernel.DeepSeek.Core.Models;

/// <summary>
/// DashScope text generation request object.
/// </summary>
internal sealed class ChatCompletionRequest
{
    /// <summary>
    /// This is the default name when using deepseek-chat and will be ignored as the deepseek-chat will only target the current activated model.
    /// </summary>
    private const string TextGenerationInferenceDefaultModel = "deepseek-chat";

    [JsonPropertyName("messages")]
    public IList<ChatWithDataMessage> Messages { get; set; } = [];

    [JsonPropertyName("model")]
    public string Model { get; set; }

    [JsonPropertyName("frequency_penalty")]
    public double FrequencyPenalty { get; set; } = 0;

    [JsonPropertyName("max_tokens")]
    public int? MaxTokens { get; set; }

    [JsonPropertyName("presence_penalty")]
    public double PresencePenalty { get; set; } = 0;

    [JsonPropertyName("stream")]
    public bool IsStreamEnabled { get; set; }

    [JsonPropertyName("temperature")]
    public double Temperature { get; set; } = 0;

    [JsonPropertyName("top_p")]
    public double TopP { get; set; } = 0;

    [JsonPropertyName("logprobs")]
    public bool? Logprobs { get; set; }

    [JsonPropertyName("top_logprobs")]
    public int? TopLogprobs { get; set; }


    /// <summary>
    /// Converts a <see cref="PromptExecutionSettings" /> object to a <see cref="TextGenerationRequest" /> object.
    /// </summary>
    /// <param name="chatHistory">Chat history to be used for the request.</param>
    /// <param name="executionSettings">Execution settings to be used for the request.</param>
    /// <returns>TexGenerationtRequest object.</returns>
    internal static ChatCompletionRequest FromChatHistoryAndExecutionSettings(ChatHistory chatHistory, DeepSeekPromptExecutionSettings executionSettings, string deploymentName)
    {
        return new ChatCompletionRequest
        {
            Messages = chatHistory.Select(message => new ChatWithDataMessage
            {
                Content = message.Content,
                Role = message.Role.ToString(),
            }).ToList(),

            Model = deploymentName ?? executionSettings.ModelId ?? TextGenerationInferenceDefaultModel,
            FrequencyPenalty = executionSettings.FrequencyPenalty,
            MaxTokens = executionSettings.MaxTokens,
            PresencePenalty = executionSettings.PresencePenalty,
            Temperature = executionSettings.Temperature,
            TopP = executionSettings.TopP,
            Logprobs = executionSettings.Logprobs ?? false,
            TopLogprobs = executionSettings.TopLogprobs,
        };
    }


}
internal sealed class ChatWithDataMessage
{
    [JsonPropertyName("role")]
    public string Role { get; set; }

    [JsonPropertyName("content")]
    public string Content { get; set; }
}