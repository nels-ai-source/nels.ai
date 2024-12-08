namespace Nels.SemanticKernel.Process.Variables.Schemas;

public class StringVariableSchema : VariableSchema, IVariableSchema
{
    public string Type { get; set; } = VariableTypeConst.String;
}
