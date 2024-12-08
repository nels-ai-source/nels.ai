using System.Text.Json.Serialization;

namespace Nels.SemanticKernel.Baidu.Core.Models;

internal sealed class ChatCompletionStreamResponse
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("object")]
    public string Object { get; set; }

    [JsonPropertyName("created")]
    public long Created { get; set; }

    [JsonPropertyName("sentence_id")]
    public int Sentence_id { get; set; }

    [JsonPropertyName("result")]
    public string result { get; set; }

    [JsonPropertyName("need_clear_history")]
    public bool need_clear_history { get; set; }

    [JsonPropertyName("finish_reason")]
    public string finish_reason { get; set; }

    public Usage usage { get; set; }

}

internal sealed class Usage
{
    public int prompt_tokens { get; set; }

    public int completion_tokens { get; set; }

    public int total_tokens { get; set; }
}
