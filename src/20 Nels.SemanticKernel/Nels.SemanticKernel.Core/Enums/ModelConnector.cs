using System.ComponentModel;

namespace Nels.SemanticKernel.Enums;

public enum ModelConnector
{
    [Description("OpenAI")]
    OpenAI = 1,
    [Description("AzureOpenAI")]
    AzureOpenAI = 2,
    [Description("Google")]
    Google = 3,
    [Description("HuggingFace")]
    HuggingFace = 4,
    [Description("MistralAI")]
    MistralAI = 5,
    [Description("Ollama")]
    Ollama = 6,
    [Description("Onnx")]
    Onnx = 7,
    [Description("Amazon")]
    Amazon = 8,
}
