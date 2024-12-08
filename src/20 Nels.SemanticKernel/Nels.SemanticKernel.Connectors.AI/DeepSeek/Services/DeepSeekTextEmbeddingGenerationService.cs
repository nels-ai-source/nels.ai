using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Embeddings;
using Microsoft.SemanticKernel.Services;
using Nels.SemanticKernel.DashScope.Core;
using Nels.SemanticKernel.InternalUtilities.Http;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Nels.SemanticKernel.DeepSeek.Services;

public class DeepSeekTextEmbeddingGenerationService : ITextEmbeddingGenerationService
{
    private Dictionary<string, object> AttributesInternal { get; } = new Dictionary<string, object>();
    private DashScopeClient Client { get; }

    /// <inheritdoc />
    public IReadOnlyDictionary<string, object> Attributes => AttributesInternal;

    public DeepSeekTextEmbeddingGenerationService(
        string model,
        Uri endpoint = null,
        string apiKey = null,
        HttpClient httpClient = null,
        ILoggerFactory loggerFactory = null)
    {
        Client = new DashScopeClient(modelId: model, endpoint: endpoint ?? httpClient?.BaseAddress, apiKey: apiKey, httpClient: HttpClientProvider.GetHttpClient(httpClient), logger: loggerFactory?.CreateLogger(GetType())
            );
        AttributesInternal.Add(AIServiceExtensions.ModelIdKey, model);
    }

    public Task<IList<ReadOnlyMemory<float>>> GenerateEmbeddingsAsync(IList<string> data, Kernel kernel = null, CancellationToken cancellationToken = default)
        => Client.GenerateEmbeddingsAsync(data, kernel, cancellationToken);
}
