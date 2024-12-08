using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Services;
using Nels.SemanticKernel.InternalUtilities.Http;
using Nels.SemanticKernel.Kimi.Core;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Nels.SemanticKernel.Kimi.Services;

public class KimiChatCompletionService : IChatCompletionService
{
    private Dictionary<string, object> AttributesInternal { get; } = new Dictionary<string, object>();


    private readonly KimiMessageApiClient Client;

    public KimiChatCompletionService(
        string model,
        Uri endpoint = null,
        string apiKey = null,
        HttpClient httpClient = null,
        ILoggerFactory loggerFactory = null)
    {
        Verify.NotNullOrWhiteSpace(model);

        var clientEndpoint = endpoint ?? httpClient?.BaseAddress
            ?? throw new ArgumentNullException(nameof(endpoint), "Chat completion service requires a valid endpoint provided explicitly or via HTTP client base address");

        Client = new KimiMessageApiClient(
            modelId: model,
            endpoint: clientEndpoint,
            apiKey: apiKey,
            httpClient: HttpClientProvider.GetHttpClient(httpClient),
            logger: loggerFactory?.CreateLogger(GetType()) ?? NullLogger.Instance
        );

        AttributesInternal.Add(AIServiceExtensions.ModelIdKey, model);
    }


    public IReadOnlyDictionary<string, object> Attributes => AttributesInternal;
    /// <inheritdoc />
    public Task<IReadOnlyList<ChatMessageContent>> GetChatMessageContentsAsync(ChatHistory chatHistory, PromptExecutionSettings executionSettings = null, Kernel kernel = null, CancellationToken cancellationToken = default)
        => Client.CompleteChatMessageAsync(chatHistory, executionSettings, cancellationToken);

    /// <inheritdoc />
    public IAsyncEnumerable<StreamingChatMessageContent> GetStreamingChatMessageContentsAsync(ChatHistory chatHistory, PromptExecutionSettings executionSettings = null, Kernel kernel = null, CancellationToken cancellationToken = default)
        => Client.StreamCompleteChatMessageAsync(chatHistory, executionSettings, cancellationToken);
}
