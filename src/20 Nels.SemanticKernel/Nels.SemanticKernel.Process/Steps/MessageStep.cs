using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.PromptTemplates.Handlebars;
using Microsoft.SemanticKernel.PromptTemplates.Liquid;
using Nels.SemanticKernel.Interfaces;
using Nels.SemanticKernel.Process.Extensions;
using Nels.SemanticKernel.Process.States;
using Nels.SemanticKernel.Process.Templates;
using System.Text.Json.Serialization;

namespace Nels.SemanticKernel.Process.Steps;

public class MessageStep(IStreamResponse streamResponse) : NelsKernelProcessStep<MessageStepState>
{
    private readonly IStreamResponse _streamResponse = streamResponse;
    private string _result;

    [KernelFunction(StepTypeConst.Message)]
    public async ValueTask ExecuteAsync(KernelProcessStepContext context, Dictionary<string, object> content, CancellationToken cancellationToken)
    {
        await base.StepExecuteAsync(context, content, cancellationToken);
    }
    public override async ValueTask<bool> PreExecuteAsync(CancellationToken cancellationToken)
    {
        var result = await base.PreExecuteAsync(cancellationToken);
        if (result == false) return false;
        var template = TemplateReplace(_state.Template, _state.Arguments);
        await _streamResponse.WriteDataAsync(ProcessEventType.Message_Template, new ProcessEventData(_id, nameof(_state.Template), template).Properties);
        return result;
    }
    public override ValueTask PostExecuteAsync(CancellationToken cancellationToken)
    {
        var arguments = _state.Arguments.ToDictionary() ?? [];
        if (_state.TemplateFormat == LiquidPromptTemplateFactory.LiquidTemplateFormat)
        {
            _result = LiquidTemplate.Render(_state.Template, arguments);
        }
        else if (_state.TemplateFormat == HandlebarsPromptTemplateFactory.HandlebarsTemplateFormat)
        {
            _result = HandlebarsTemplate.Render(_state.Template, arguments);
        }
        _state.Context.AddDefaultOutput(_id, _result);
        return base.PostExecuteAsync(cancellationToken);
    }

    public override async ValueTask OnChatMessageAsync(LlmChatMessageEventData data)
    {
        var input = _state.Inputs.FirstOrDefault(x => x.IdEqual(data.Id));
        if (input == null) return;
        await _streamResponse.WriteDataAsync(ProcessEventType.Data, new ProcessEventData(_id, input.Name, data.Content).Properties);
    }
    private static string TemplateReplace(string promptTemplateText, KernelArguments arguments)
    {
        foreach (var item in arguments)
        {
            string? replacement = item.Value != null && IsPrimitiveType(item.Value.GetType())
                ? item.Value.ToString() : null;
            if (replacement == null) continue;
            promptTemplateText = promptTemplateText.Replace("{{" + item.Key + "}}", replacement ?? string.Empty);

        }

        return promptTemplateText;
    }
    private static bool IsPrimitiveType(Type type)
    {
        return type.IsPrimitive || type == typeof(string) || type == typeof(decimal) ||
               type == typeof(DateTime) || type == typeof(TimeSpan);
    }
}
public class MessageStepState : StepState, IInputState
{
    [JsonPropertyName("inputs")]
    public List<Variables.InputVariable> Inputs { get; set; } = [];

    [JsonPropertyName("streamingOutput")]
    public bool StreamingOutput { get; set; }

    [JsonPropertyName("template")]
    public string Template { get; set; }

    [JsonPropertyName("template_format")]
    public string TemplateFormat { get; set; } = LiquidPromptTemplateFactory.LiquidTemplateFormat;
}
