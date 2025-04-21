using System.ComponentModel;

namespace Nels.SemanticKernel.Enums;

public enum ModelConnector
{
    [Description("OpenAI")]
    OpenAI = 0,
    [Description("AzureOpenAI")]
    AzureOpenAI = 1,
    [Description("Google")]
    Google = 2,
    [Description("HuggingFace")]
    HuggingFace = 3,
    [Description("MistralAI")]
    MistralAI = 4,
    [Description("Ollama")]
    Ollama = 5,
    [Description("Onnx")]
    Onnx = 6,
    [Description("Amazon")]
    Amazon = 6,
}
