namespace Nels.SemanticKernel.Process.Variables.Schemas;

public class ObjectVariableSchema : VariableSchema, IVariableSchema
{
    public string Type { get; set; } = VariableTypeConst.Object;
    public List<IVariableSchema> Schema { get; set; }
}
