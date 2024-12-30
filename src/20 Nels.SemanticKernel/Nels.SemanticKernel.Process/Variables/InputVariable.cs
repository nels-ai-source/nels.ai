namespace Nels.SemanticKernel.Process.Variables;

public class InputVariable : Variable
{
    public VariableValue Value { get; set; }

    public object GetValue(Dictionary<string, object> context)
    {
        return Value.GetValue(context);
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

