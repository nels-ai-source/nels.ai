namespace Nels.SemanticKernel.Process.Variables.Schemas;

public class ArrayIntegerVariableSchema : VariableSchema, IVariableSchema
{
    public string Type { get; set; } = VariableTypeConst.ArrayInteger;
}
