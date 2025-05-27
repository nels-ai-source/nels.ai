using System.ComponentModel;

namespace Nels.SemanticKernel.Enums;

public enum ModelCapability
{
    [Description("TextGeneration")]
    TextGeneration = 1,
    [Description("ImageComprehend")]
    ImageComprehend = 2,
    [Description("AudioComprehend")]
    AudioComprehend = 3,
    [Description("VideoComprehend")]
    VideoComprehend = 4,
    [Description("Embedding")]
    Embedding = 5,

    [Description("Reasoning")]
    Reasoning = 6,
    [Description("FunctionCall")]
    FunctionCall = 7,
    [Description("JsonOutput")]
    JsonOutput = 8,
}
