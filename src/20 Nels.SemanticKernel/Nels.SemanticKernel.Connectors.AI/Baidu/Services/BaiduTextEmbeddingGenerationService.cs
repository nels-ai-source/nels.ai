using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Embeddings;
using Microsoft.SemanticKernel.Services;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Nels.SemanticKernel.Baidu.Core;
using Nels.SemanticKernel.InternalUtilities.Http;

namespace Nels.SemanticKernel.Baidu.Services;

public class BaiduTextEmbeddingGenerationService : ITextEmbeddingGenerationService
{

    private Dictionary<string, object> AttributesInternal { get; } = new Dictionary<string, object>();
    private BaiduClient Client { get; }

    /// <inheritdoc />
    public IReadOnlyDictionary<string, object> Attributes => AttributesInternal;

    public BaiduTextEmbeddingGenerationService(
        string model,
        Uri endpoint = null,
        string apiKey = null,
        string clientId = null,
        HttpClient httpClient = null,
        ILoggerFactory loggerFactory = null)
    {
        Client = new BaiduClient(modelId: model, endpoint: endpoint ?? httpClient?.BaseAddress, apiKey: apiKey, clientId: clientId, httpClient: HttpClientProvider.GetHttpClient(httpClient), logger: loggerFactory?.CreateLogger(GetType())
            );
        AttributesInternal.Add(AIServiceExtensions.ModelIdKey, model);
    }

    public Task<IList<ReadOnlyMemory<float>>> GenerateEmbeddingsAsync(IList<string> data, Kernel kernel = null, CancellationToken cancellationToken = default)
        => Client.GenerateEmbeddingsAsync(data, kernel, cancellationToken);
}
