using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Services;
using Nels.SemanticKernel.DashScope.Core;
using Nels.SemanticKernel.InternalUtilities.Http;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Nels.SemanticKernel.DashScope.Services;

public class DashScopeChatCompletionService : IChatCompletionService
{
    private Dictionary<string, object> AttributesInternal { get; } = [];

    private readonly DashScopeClient Client;

    public DashScopeChatCompletionService(
        string model,
        Uri endpoint = null,
        string apiKey = null,
        HttpClient httpClient = null,
        ILoggerFactory loggerFactory = null)
    {
        Verify.NotNullOrWhiteSpace(model);

        var clientEndpoint = endpoint ?? httpClient?.BaseAddress
            ?? throw new ArgumentNullException(nameof(endpoint), "Chat completion service requires a valid endpoint provided explicitly or via HTTP client base address");

        Client = new DashScopeClient(
            modelId: model,
            endpoint: clientEndpoint,
            apiKey: apiKey,
            httpClient: HttpClientProvider.GetHttpClient(httpClient),
            logger: loggerFactory?.CreateLogger(GetType()) ?? NullLogger.Instance
        );

        AttributesInternal.Add(AIServiceExtensions.ModelIdKey, model);
    }
    /// <inheritdoc />
    public IReadOnlyDictionary<string, object> Attributes => AttributesInternal;
    /// <inheritdoc />
    public Task<IReadOnlyList<ChatMessageContent>> GetChatMessageContentsAsync(ChatHistory chatHistory, PromptExecutionSettings executionSettings = null, Kernel kernel = null, CancellationToken cancellationToken = default)
        => Client.CompleteChatMessageAsync(chatHistory, executionSettings, kernel, cancellationToken);

    /// <inheritdoc />
    public IAsyncEnumerable<StreamingChatMessageContent> GetStreamingChatMessageContentsAsync(ChatHistory chatHistory, PromptExecutionSettings executionSettings = null, Kernel kernel = null, CancellationToken cancellationToken = default)
        => Client.StreamCompleteChatMessageAsync(chatHistory, executionSettings, kernel, cancellationToken);
}
