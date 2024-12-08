using Microsoft.SemanticKernel.ChatCompletion;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace Nels.SemanticKernel.Kimi.Core.Models;

internal sealed class KimiChatCompletionRequest
{

    /// <summary>
    /// Model ID moonshot-v1-8k,moonshot-v1-32k,moonshot-v1-128k 其一
    /// </summary>
    [JsonPropertyName("model")]
    public string Model { get; set; }


    /// <summary>
    /// 聊天完成时生成的最大 token 数
    /// </summary>
    [JsonPropertyName("max_tokens")]
    public int? MaxTokens { get; set; }


    /// <summary>
    /// 使用什么采样温度，介于 0 和 1 之间
    /// </summary>
    [JsonPropertyName("temperature")]
    public double? Temperature { get; set; } = 0.3;

    /// <summary>
    /// 为每条输入消息生成多少个结果
    /// </summary>
    [JsonPropertyName("n")]
    public int? n { get; set; } = 1;


    /// <summary>
    /// 停止词，当全匹配这个（组）词后会停止输出，这个（组）词本身不会输出。最多不能超过 5 个字符串，每个字符串不得超过 32 字节
    /// </summary>
    [JsonPropertyName("stop")]
    public List<string> Stop { get; set; }

    /// <summary>
    /// 是否流式返回
    /// </summary>
    [JsonPropertyName("stream")]
    public bool Stream { get; set; } = false;

    /// <summary>
    /// 包含迄今为止对话的消息列表
    /// </summary>
    [JsonPropertyName("messages")]
    public List<ChatMessage> Messages { get; set; }


    internal sealed class ChatMessage
    {
        [JsonPropertyName("role")]
        public string Role { get; set; }

        [JsonPropertyName("content")]
        public string Content { get; set; }
    }


    internal static KimiChatCompletionRequest FromChatHistoryAndExecutionSettings(ChatHistory chatHistory, KimiPromptExecutionSettings executionSettings)
    {
        return new KimiChatCompletionRequest
        {
            Model = executionSettings.ModelId,
            Messages = chatHistory.Select(message => new ChatMessage
            {
                Content = message.Content,
                Role = message.Role.ToString(),
            }).ToList(),
            Temperature = executionSettings.Temperature,
            MaxTokens = executionSettings.MaxTokens.Value,
            n = executionSettings.ResultsPerPrompt,
            Stop = executionSettings.Stop,
            Stream = true
        };
    }
}
