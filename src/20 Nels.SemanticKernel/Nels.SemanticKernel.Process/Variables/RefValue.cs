using static Nels.SemanticKernel.Process.Steps.LlmStep;

namespace Nels.SemanticKernel.Process.Variables;

public class RefValue : IVariableValue
{
    public string Type { get; } = VariableValueTypeConst.Ref;
    public VariableRefConent Content { get; set; }

    public object GetValue(StepState state)
    {
        if (state.Context.TryGetValue(Content.Id, out var values) == false) return string.Empty;
        if (values is LlmGetStreamingChatMessage message) return message;
        if (values is Dictionary<string, object> keyValues)
        {
            if (keyValues.TryGetValue(Content.Name, out var value) == false) return string.Empty;
            return value;
        }
        return values;
    }

    public bool IdEqual(string id)
    {
        return Content.Id == id;
    }
}
public class VariableRefConent
{
    public string Id { get; set; }
    public string Name { get; set; }
}
