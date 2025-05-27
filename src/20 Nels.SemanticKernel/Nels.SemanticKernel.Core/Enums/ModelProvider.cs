using System.ComponentModel;

namespace Nels.SemanticKernel.Enums;

public enum ModelProvider
{
    [Description("OpenAI")]
    OpenAI = 1,
    [Description("AzureOpenAI")]
    AzureOpenAI = 2,
    [Description("Anthropic")]
    Anthropic = 3,
    [Description("Google")]
    Google = 4,
    [Description("DashScope")]
    DashScope = 100,
    [Description("DeepSeek")]
    DeepSeek = 101,
}

