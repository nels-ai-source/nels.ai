using Microsoft.SemanticKernel;
using Nels.SemanticKernel.Process.Logs;
using System.Diagnostics;
using System.Text.Json.Serialization;

namespace Nels.SemanticKernel.Process
{
    public class StepState
    {
        public string Id { get; set; }
        public KernelArguments Arguments { get; set; } = [];

        [JsonIgnore]
        public IDictionary<object, object> Context { get; set; }

        [JsonIgnore]
        public StepLog StepLog { get; set; } = new();

        [JsonIgnore]
        public Stopwatch Stopwatch { get; set; } = new();
    }
}
