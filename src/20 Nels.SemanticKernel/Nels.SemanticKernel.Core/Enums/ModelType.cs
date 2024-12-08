using System.ComponentModel;

namespace Nels.SemanticKernel.Enums;

public enum ModelType
{
    [Description("TextGeneration")]
    TextGeneration = 0,
    [Description("Embedding")]
    Embedding = 1
}