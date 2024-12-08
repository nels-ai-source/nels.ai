using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Nels.SemanticKernel.DashScope.Core.Models;

/// <summary>
/// HTTP schema to perform embedding request.
/// </summary>
internal sealed class TextEmbeddingRequest
{
    /// <summary>
    /// model
    /// </summary>
    [JsonPropertyName("model")]
    public string Model { get; set; }
    /// <summary>
    /// parameters
    /// </summary>
    [JsonPropertyName("input")]
    public EmbeddingInput Input { get; set; }
    /// <summary>
    /// parameters
    /// </summary>
    [JsonPropertyName("parameters")]
    public EmbeddingParameter Parameters { get; set; } = new EmbeddingParameter();
}
internal sealed class EmbeddingInput
{
    /// <summary>
    /// Data to embed.
    /// </summary>
    [JsonPropertyName("texts")]
    public IList<string> Texts { get; set; } = new List<string>();
}
internal sealed class EmbeddingParameter
{
    [JsonPropertyName("text_type")]
    public string TextType { get; set; } = "document";
}
