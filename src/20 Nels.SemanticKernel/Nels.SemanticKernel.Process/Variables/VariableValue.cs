using Nels.SemanticKernel.Process.Logs;
using static Nels.SemanticKernel.Process.Steps.LlmStep;

namespace Nels.SemanticKernel.Process.Variables;

public class VariableValue
{
    public string Type { get; set; } = VariableValueTypeConst.Ref;
    public string Content { get; set; }
    public string RefKey { get; set; }

    public object GetValue(StepState state)
    {
        if (Type == VariableValueTypeConst.Ref)
        {
            return GetRefValue(state);
        }

        return Content;
    }

    private object GetRefValue(StepState state)
    {
        if (state.Context.TryGetValue(RefKey, out var values) == false) return string.Empty;
        if (values is LlmGetStreamingChatMessage message) return message;
        if (values is Dictionary<string, object> keyValues)
        {
            if (keyValues.TryGetValue(Content, out var value) == false) return string.Empty;
            return value;
        }
        return values;
    }

    public bool IdEqual(string id)
    {
        if (Type == VariableValueTypeConst.Ref)
        {
            return RefKey == id;
        }
        return false;
    }
}
