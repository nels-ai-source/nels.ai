using Nels.SemanticKernel.Process.Logs;

namespace Nels.SemanticKernel.Process.Variables;

public class InputVariable : Variable
{
    public VariableValue Value { get; set; }

    public object GetValue(StepState state)
    {
        return Value.GetValue(state);
    }
    public bool IdEqual(string id)
    {
        return Value.IdEqual(id);
    }
}
public class OutputVariable : Variable
{

}
public class Variable
{
    public string Name { get; set; }
    public string Type { get; set; }
    public string Description { get; set; }
}

