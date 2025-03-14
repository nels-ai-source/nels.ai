// Copyright (c) Microsoft. All rights reserved.

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Http;
using Nels.SemanticKernel.DashScope.Core.Models;
using Nels.SemanticKernel.InternalUtilities.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Nels.SemanticKernel.DashScope.Core;

internal partial class DashScopeClient
{
    public async Task<IList<ReadOnlyMemory<float>>> GenerateEmbeddingsAsync(IList<string> data, Kernel kernel, CancellationToken cancellationToken)
    {
        var endpoint = GetEmbeddingGenerationEndpoint();

        if (data.Count > 1)
        {
            throw new NotSupportedException("Currently this interface does not support multiple embeddings results per data item, use only one data item");
        }

        var request = new DashScopeTextEmbeddingRequest
        {
            Model = ModelId,
            Input = new EmbeddingInput { Texts = data }
        };
        using var httpRequestMessage = CreatePost(request, endpoint, ApiKey);

        string body = await SendRequestAndGetStringBodyAsync(httpRequestMessage, cancellationToken)
            .ConfigureAwait(false);

        var response = DeserializeResponse<DashScopeTextEmbeddingResponse>(body);

        // Currently only one embedding per data is supported
        return response.Output.Embeddings.Select(embedding => embedding.Embedding).ToList();
    }
    private Uri GetEmbeddingGenerationEndpoint() => new($"{Endpoint}{Separator}api/v1/services/embeddings/text-embedding/text-embedding");
}
