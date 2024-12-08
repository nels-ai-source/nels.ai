namespace Nels.SemanticKernel.Process.Variables.Schemas;

public class ArrayNumberVariableSchema : VariableSchema, IVariableSchema
{
    public string Type { get; set; } = VariableTypeConst.ArrayNumber;
}
