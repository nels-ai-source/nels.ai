using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Nels.SemanticKernel.DashScope.Core.Models;

/// <summary>
/// Represents the response from the Qwen text embedding API.
/// </summary>
internal sealed class TextEmbeddingResponse
{
    [JsonPropertyName("output")]
    [JsonRequired]
    public EmbeddingOutput Output { get; set; } = null!;

    [JsonPropertyName("usage")]
    [JsonRequired]
    public EmbeddingUsage Usage { get; set; } = null!;

    [JsonPropertyName("request_id")]
    [JsonRequired]
    public string RequestId { get; set; } = null!;
}
internal sealed class EmbeddingOutput
{
    [JsonPropertyName("embeddings")]
    [JsonRequired]
    public List<Embeddings> Embeddings { get; set; } = null!;
}
internal sealed class Embeddings
{
    [JsonPropertyName("text_index")]
    [JsonRequired]
    public int TextIndex { get; set; }

    [JsonPropertyName("embedding")]
    [JsonRequired]
    public ReadOnlyMemory<float> Embedding { get; set; }
}
internal sealed class EmbeddingUsage
{
    [JsonPropertyName("total_tokens")]
    [JsonRequired]
    public int TotalTokens { get; set; }
}
