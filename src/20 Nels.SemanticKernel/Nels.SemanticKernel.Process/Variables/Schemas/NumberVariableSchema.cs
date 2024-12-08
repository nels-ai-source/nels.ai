namespace Nels.SemanticKernel.Process.Variables.Schemas;

public class NumberVariableSchema : VariableSchema, IVariableSchema
{
    public string Type { get; set; } = VariableTypeConst.Number;
}
