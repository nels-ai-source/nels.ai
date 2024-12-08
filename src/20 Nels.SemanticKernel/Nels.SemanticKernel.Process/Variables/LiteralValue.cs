namespace Nels.SemanticKernel.Process.Variables;

public class LiteralValue : IVariableValue
{
    public string Type { get; } = VariableValueTypeConst.Literal;
    public string Content { get; set; }

    public object GetValue(StepState state)
    {
        return Content;
    }

    public bool IdEqual(string id)
    {
        return false;
    }
}
