namespace Nels.SemanticKernel.Process.Variables.Schemas;

public class BooleanVariableSchema : VariableSchema, IVariableSchema
{
    public string Type { get; set; } = VariableTypeConst.Boolean;
}
