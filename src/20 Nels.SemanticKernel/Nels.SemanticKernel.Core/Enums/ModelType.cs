using System.ComponentModel;

namespace Nels.SemanticKernel.Enums;

public enum ModelType
{
    [Description("TextGeneration")]
    TextGeneration = 1,
    [Description("Embedding")]
    Embedding = 2,
    [Description("MultiModal")]
    MultiModal =3,
}

