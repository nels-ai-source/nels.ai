using Nels.SemanticKernel.Process.Variables;

namespace Nels.SemanticKernel.Process.States;

public interface IInputState
{
    List<InputVariable> Inputs { get; set; }
}
