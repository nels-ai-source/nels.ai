namespace Nels.SemanticKernel.Process.Variables.Schemas;

public class ArrayObjectVariableSchema : VariableSchema, IVariableSchema
{
    public string Type { get; set; } = VariableTypeConst.ArrayObject;
    public List<IVariableSchema> Schema { get; set; }
}
