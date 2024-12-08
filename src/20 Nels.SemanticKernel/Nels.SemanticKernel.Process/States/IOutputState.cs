using Nels.SemanticKernel.Process.Variables;

namespace Nels.SemanticKernel.Process.States;

public interface IOutputState
{
    List<OutputVariable> Outputs { get; set; }
}
