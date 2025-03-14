using Microsoft.SemanticKernel.ChatCompletion;
using Nels.SemanticKernel.DashScope;
using Nels.SemanticKernel.DashScope.Core;
using Nels.SemanticKernel.DashScope.Core.Models;
using System;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;
using Assert = Xunit.Assert;

namespace Nels.SemanticKernel.Connectors.AI.UnitTests.DashScope.Core;

public sealed class DashScopeClientTests : IDisposable
{
    private readonly HttpClient _httpClient;
    private readonly HttpMessageHandlerStub _messageHandlerStub;
    private readonly string _responseContentFinishReasonOther;
    private const string StreamTestDataFilePath = "./TestData/chat_stream_response.json";
    private const string StreamTestDataFinishReasonOtherFilePath = "./TestData/chat_stream_finish_reason_other_response.json";

    public DashScopeClientTests()
    {
        this._responseContentFinishReasonOther = File.ReadAllText(StreamTestDataFinishReasonOtherFilePath);
        this._messageHandlerStub = new HttpMessageHandlerStub();
        this._messageHandlerStub.ResponseToReturn.Content = new StringContent(
            File.ReadAllText(StreamTestDataFilePath));

        this._httpClient = new HttpClient(this._messageHandlerStub, false);
    }

    [Fact]
    public async Task ShouldUsePromptExecutionSettingsAsync()
    {
        // Arrange
        var client = this.CreateChatCompletionClient();
        var chatHistory = CreateSampleChatHistory();
        var executionSettings = new DashScopePromptExecutionSettings()
        {
            MaxTokens = 102,
            Temperature = 0.45F,
            TopP = 0.6F
        };

        // Act
        await client.CompleteChatMessageAsync(chatHistory, executionSettings);

        // Assert
        var dashScopeRequest = JsonSerializer.Deserialize<DashScopeChatCompletionRequest>(this._messageHandlerStub.RequestContent);
        Assert.NotNull(dashScopeRequest);
        Assert.Equal(executionSettings.MaxTokens, dashScopeRequest.Parameters!.MaxTokens);
        Assert.Equal(executionSettings.Temperature, dashScopeRequest.Parameters!.Temperature);
        Assert.Equal(executionSettings.TopP, dashScopeRequest.Parameters!.TopP);
    }

    private static ChatHistory CreateSampleChatHistory()
    {
        var chatHistory = new ChatHistory();
        chatHistory.AddUserMessage("Hello");
        chatHistory.AddAssistantMessage("Hi");
        chatHistory.AddUserMessage("How are you?");
        return chatHistory;
    }

    private DashScopeClient CreateChatCompletionClient(
        string modelId = "fake-model",
        string? bearerKey = null,
        HttpClient? httpClient = null)
    {
        return new DashScopeClient(
            httpClient: httpClient ?? this._httpClient,
            modelId: modelId,
            apiKey: "fake-key");
    }

    public void Dispose()
    {
        this._httpClient.Dispose();
    }
}
