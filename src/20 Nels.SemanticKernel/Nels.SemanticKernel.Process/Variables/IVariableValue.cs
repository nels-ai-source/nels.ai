namespace Nels.SemanticKernel.Process.Variables;

public interface IVariableValue
{
    string Type { get; }
    object GetValue(StepState state);

    bool IdEqual(string id);
}

