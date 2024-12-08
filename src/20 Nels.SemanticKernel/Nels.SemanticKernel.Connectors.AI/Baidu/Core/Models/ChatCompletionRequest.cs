using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

namespace Nels.SemanticKernel.Baidu.Core.Models;

internal sealed class ChatCompletionRequest
{
    /// <summary>
    /// This is the default name when using TGI and will be ignored as the TGI will only target the current activated model.
    /// </summary>
    private const string TextGenerationInferenceDefaultModel = "tgi";
    /// <summary>
    /// Model name to use for generation.
    /// </summary>
    /// <remarks>
    /// When using TGI this parameter will be ignored.
    /// </remarks>
    [JsonPropertyName("model")]
    public string Model { get; set; }


    /// <summary>
    /// Indicates whether to get the response as stream or not.
    /// </summary>
    [JsonPropertyName("stream")]
    public bool Stream { get; set; }


    /// <summary>
    /// 温度 默认0.5
    /// </summary>
    [JsonPropertyName("temperature")]
    public float Temperature { get; set; } = 0.5F;


    /// <summary>
    /// 模型回答的tokens的最大长度
    /// </summary>
    [JsonPropertyName("max_tokens")]
    public int Max_tokens { get; set; } = 4096;

    /// <summary>
    /// 从k个候选中随机选择⼀个（⾮等概率） 取值为[1，6],默认为4
    /// </summary>
    [JsonPropertyName("top_k")]
    public int Top_k { get; set; } = 4;

    [JsonPropertyName("messages")]
    public List<Message> Messages { get; set; }

    /// <summary>
    /// Converts a <see cref="PromptExecutionSettings" /> object to a <see cref="TextGenerationRequest" /> object.
    /// </summary>
    /// <param name="chatHistory">Chat history to be used for the request.</param>
    /// <param name="executionSettings">Execution settings to be used for the request.</param>
    /// <returns>TexGenerationtRequest object.</returns>
    internal static ChatCompletionRequest FromChatHistoryAndExecutionSettings(ChatHistory chatHistory, BaiduPromptExecutionSettings executionSettings)
    {
        ChatCompletionRequest chatCompletionRequest = new ChatCompletionRequest();

        List<Message> messages = chatHistory.Select(message => new Message
        {
            Content = message.Content,
            Role = message.Role.ToString(),
        }).ToList();


        chatCompletionRequest.Messages = messages;
        chatCompletionRequest.Top_k = executionSettings.TopK.Value;
        chatCompletionRequest.Temperature = executionSettings.Temperature;
        chatCompletionRequest.Max_tokens = executionSettings.MaxTokens.Value;
        return chatCompletionRequest;
    }





    internal sealed class Message
    {
        [JsonPropertyName("role")]
        public string Role { get; set; }

        [JsonPropertyName("content")]
        public string Content { get; set; }

    }
}
