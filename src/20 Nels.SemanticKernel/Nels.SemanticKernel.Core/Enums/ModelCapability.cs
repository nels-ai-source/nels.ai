using System.ComponentModel;

namespace Nels.SemanticKernel.Enums;

public enum ModelCapability
{
    [Description("TextGeneration")]
    TextGeneration = 0,
    [Description("ImageComprehend")]
    ImageComprehend = 1,
    [Description("AudioComprehend")]
    AudioComprehend = 2,
    [Description("VideoComprehend")]
    VideoComprehend = 3,
    [Description("Embedding")]
    Embedding = 4,

    [Description("Reasoning")]
    Reasoning = 51,
    [Description("FunctionCall")]
    FunctionCall = 52,
    [Description("JsonOutput")]
    JsonOutput = 53,
}
