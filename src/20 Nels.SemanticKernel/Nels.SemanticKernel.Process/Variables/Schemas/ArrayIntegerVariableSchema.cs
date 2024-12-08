namespace Nels.SemanticKernel.Process.Variables.Schemas;

public class ArrayBooleanVariableSchema : VariableSchema, IVariableSchema
{
    public string Type { get; set; } = VariableTypeConst.ArrayBoolean;
}
