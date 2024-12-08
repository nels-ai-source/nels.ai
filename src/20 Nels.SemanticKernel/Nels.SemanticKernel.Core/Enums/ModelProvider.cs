using System.ComponentModel;

namespace Nels.SemanticKernel.Enums;

public enum ModelProvider
{
    [Description("OpenAI")]
    OpenAI = 0,
    [Description("AzureOpenAI")]
    AzureOpenAI = 1,
    [Description("Baidu")]
    Baidu = 20,
    [Description("DashScope")]
    DashScope = 21,
    [Description("DeepSeek")]
    DeepSeek = 22,
    [Description("Kimi")]
    Kimi = 23,
}