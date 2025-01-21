using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Nels.Aigc.Models;

public class TextModelMetadata
{
    [JsonPropertyName("maxTokens")]
    public int MaxTokens { get; set; }
}
