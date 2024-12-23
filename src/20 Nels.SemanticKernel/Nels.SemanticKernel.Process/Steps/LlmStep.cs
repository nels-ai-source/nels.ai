using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.PromptTemplates.Handlebars;
using Microsoft.SemanticKernel.PromptTemplates.Liquid;
using Nels.SemanticKernel.Extensions;
using Nels.SemanticKernel.InternalUtilities;
using Nels.SemanticKernel.InternalUtilities.Models;
using Nels.SemanticKernel.Process.Extensions;
using Nels.SemanticKernel.Process.States;
using System.Text.Json.Serialization;

namespace Nels.SemanticKernel.Process.Steps;

public class LlmStep : NelsKernelProcessStep<LlmStepState>
{
    private readonly IPromptTemplateFactory _templateFactory = new LiquidPromptTemplateFactory();
    private LlmGetStreamingChatMessage _chatDeleegate;
    private string _result;
    public delegate Task<string> LlmGetStreamingChatMessage(MessageEventHandler messageEvent);

    [KernelFunction(StepTypeConst.Llm)]
    public async ValueTask ExecuteAsync(KernelProcessStepContext context, Kernel kernel, CancellationToken cancellationToken)
    {
        await base.StepExecuteAsync(context, kernel, cancellationToken);
    }

    public override async ValueTask<bool> ExecutionAsync(CancellationToken cancellationToken)
    {
        _chatDeleegate = new LlmGetStreamingChatMessage(GetStreamingChatMessageAsync);
        _processState.Context.AddOrUpdate(_id, _chatDeleegate);
        await base.ExecutionAsync(cancellationToken);

        return false;
    }

    public override async ValueTask PostExecuteAsync(CancellationToken cancellationToken)
    {
        if (_state.ResponseFormat == ResponseFormatConst.Json)
        {
            var resultDictionary = JsonResultTranslator.Translate<Dictionary<string, object>>(_result) ?? [];
            _processState.Context.AddOrUpdate(_id, resultDictionary);
            await base.PostExecuteAsync(cancellationToken);
            return;
        }
        _processState.Context.AddDefaultOutput(_id, _result);
        await base.PostExecuteAsync(cancellationToken);
    }

    #region private
    public delegate ValueTask MessageEventHandler(LlmChatMessageEventData data);
    public async Task<string> GetStreamingChatMessageAsync(MessageEventHandler messageEvent)
    {
        _state.Stopwatch.Start();

        var chatCompletionService = GetCompletionService() ?? throw new Exception();
        var promptExecutionSettings = new PromptExecutionSettings() { ExtensionData = _state.ExtensionData };

        var chatHistory = await GetChatMessagesAsync();

        _result = string.Empty;
        await foreach (var item in chatCompletionService.GetStreamingChatMessageContentsAsync(chatHistory, promptExecutionSettings))
        {
            if (string.IsNullOrWhiteSpace(item.Content)) continue;

            _result += item.Content;
            _stepLog.PromptTokens = (item.Metadata != null && item.Metadata.TryGetValue(nameof(ChatCompletionMetadata.UsagePromptTokens), out var usagePromptTokens)
                && int.TryParse(usagePromptTokens?.ToString(), out var promptTokens)) ? promptTokens : _stepLog.PromptTokens + 1;
            _stepLog.CompleteTokens = (item.Metadata != null && item.Metadata.TryGetValue(nameof(ChatCompletionMetadata.UsageCompletionTokens), out var usageCompletionTokens)
                && int.TryParse(usageCompletionTokens?.ToString(), out var completionTokens)) ? completionTokens : _stepLog.CompleteTokens + 1;

            await messageEvent(new LlmChatMessageEventData(_id, item.Content));
        };

        AddChatHistory(AuthorRole.Assistant, _result);

        _state.Stopwatch.Stop();

        await PostExecuteAsync(_cancellationToken);

        return _result;
    }
    private IChatCompletionService GetCompletionService()
    {
        if (string.IsNullOrWhiteSpace(_state.ModelId) == false)
        {
            return _kernel.GetRequiredService<IChatCompletionService>(_state.ModelId);
        }
        return _kernel.GetRequiredService<IChatCompletionService>();
    }
    private async Task<ChatHistory> GetChatMessagesAsync()
    {
        var chatHistory = new ChatHistory();
        foreach (var message in _state.ChatMessages)
        {
            if (string.IsNullOrWhiteSpace(message.Content))
            {
                continue;
            }
            string prompt = await RenderTemplateAsync(message.Content);
            chatHistory.AddMessage(new AuthorRole(message.Role), prompt);
        }

        foreach (var item in _state.ChatHistory)
        {
            chatHistory.Insert(chatHistory.Count - 1, new ChatMessageContent(new AuthorRole(item.Role), item.Content));
        }

        var last = chatHistory.LastOrDefault();
        if (last == null) return chatHistory;

        if (last.Role == AuthorRole.User)
        {
            AddChatHistory(last.Role, last?.Content ?? string.Empty);
        }

        return chatHistory;
    }

    private void AddChatHistory(AuthorRole role, string assistantMessage)
    {
        if (_state.HistoryCount == 0) return;

        _state.ChatHistory.Add(new MessageContent(role.Label, assistantMessage));

        if (_state.ChatHistory.Count > _state.HistoryCount * 2)
        {
            int elementsToRemove = _state.ChatHistory.Count - (_state.HistoryCount * 2);

            _state.ChatHistory.RemoveRange(0, elementsToRemove);
        }
    }
    private async Task<string> RenderTemplateAsync(string template)
    {
        var promptTemplateConfig = new PromptTemplateConfig()
        {
            Template = template,
            TemplateFormat = LiquidPromptTemplateFactory.LiquidTemplateFormat
        };
        var promptTemplate = _templateFactory.Create(promptTemplateConfig);
        return await promptTemplate.RenderAsync(_kernel, _state.Arguments);

    }
    private static async Task<IPromptTemplateFactory> CreatePromptTemplateFactoryAsync(string templateFormat)
    {
        if (templateFormat == LiquidPromptTemplateFactory.LiquidTemplateFormat)
        {
            return await Task.FromResult(new LiquidPromptTemplateFactory());
        }
        if (templateFormat == HandlebarsPromptTemplateFactory.HandlebarsTemplateFormat)
        {
            return await Task.FromResult(new HandlebarsPromptTemplateFactory());
        }
        return await Task.FromResult(new KernelPromptTemplateFactory());
    }
    #endregion
}
public class LlmStepState : StepState, IInputState, IOutputState
{
    #region 模型
    [JsonPropertyName("modelId")]
    public string? ModelId { get; set; }

    [JsonPropertyName("extensionData")]
    public Dictionary<string, object> ExtensionData { get; set; } = [];
    #endregion

    #region 输入
    [JsonPropertyName("inputs")]
    public List<Variables.InputVariable> Inputs { get; set; } = [];

    [JsonPropertyName("historyCount")]
    public int HistoryCount { get; set; } = 0;

    [JsonPropertyName("chatHistory")]
    public List<MessageContent> ChatHistory { get; set; } = [];
    #endregion

    #region 提示词
    [JsonPropertyName("chatMessages")]
    public List<MessageContent> ChatMessages { get; set; } = [];
    #endregion

    #region 输出
    [JsonPropertyName("outputs")]
    public List<Variables.OutputVariable> Outputs { get; set; } = [];

    [JsonPropertyName("response_format")]
    public string ResponseFormat { get; set; } = ResponseFormatConst.Text;
    #endregion
}
public class CompletionUsage
{
    public int PromptTokens { get; set; } = 0;

    public int CompletionTokens { get; set; } = 0;

    public int TotalTokens => PromptTokens + CompletionTokens;
}
public static class ResponseFormatConst
{
    public const string Text = "text";
    public const string Json = "json";
}
public class MessageEventData
{
    public string Id { get; set; }
    public string Message { get; set; }
}

public class MessageContent
{
    public MessageContent() { }
    public MessageContent(string role, string content)
    {
        Role = role;
        Content = content;
    }

    [JsonPropertyName("role")]
    public string Role { get; set; }

    [JsonPropertyName("content")]
    public string Content { get; set; }
}

public class LlmChatMessageEventData(string id, string content)
{
    public string Id { get; private set; } = id;
    public string Content { get; private set; } = content;
}