using System.Collections.Generic;

namespace Nels.SemanticKernel.Baidu.Core.Models;

/// <summary>
/// 百度大模型计算向量
/// </summary>
internal sealed class TextEmbeddingRequest
{
    public IList<string> input { get; set; }
}
