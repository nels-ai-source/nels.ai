using Microsoft.SemanticKernel;

namespace Nels.SemanticKernel.Process
{
    public class StepState
    {
        public Dictionary<string, object> Context { get; set; } = [];
        public KernelArguments Arguments { get; set; } = [];
        public string Id { get; set; }
    }
}
