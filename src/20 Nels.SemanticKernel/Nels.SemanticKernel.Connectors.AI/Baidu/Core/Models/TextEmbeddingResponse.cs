using System.Collections.Generic;

namespace Nels.SemanticKernel.Baidu.Core.Models;

internal sealed class TextEmbeddingResponse
{
    public string id { get; set; }

    public int created { get; set; }

    public List<EmbeddingOutput> data { get; set; }
}

internal sealed class EmbeddingOutput
{
    public List<double> embedding { get; set; }

    public int index { get; set; }
}
