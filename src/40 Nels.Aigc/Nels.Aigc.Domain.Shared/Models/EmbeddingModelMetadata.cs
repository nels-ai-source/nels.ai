using System.Text.Json.Serialization;

namespace Nels.Aigc.Models;

public class EmbeddingModelMetadata
{
    [JsonPropertyName("maxTokens")]
    public int MaxTokens { get; set; } = 512;

    [JsonPropertyName("vectorSize")]
    public int VectorSize { get; set; } = 1536;
}
