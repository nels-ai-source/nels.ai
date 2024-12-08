namespace Nels.SemanticKernel.Process.Variables.Schemas;

public class IntegerVariableSchema : VariableSchema, IVariableSchema
{
    public string Type { get; set; } = VariableTypeConst.Integer;
}
