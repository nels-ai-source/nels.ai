namespace Nels.SemanticKernel.Process.Variables.Schemas;

public class ArrayStringVariableSchema : VariableSchema, IVariableSchema
{
    public string Type { get; set; } = VariableTypeConst.ArrayString;
}
