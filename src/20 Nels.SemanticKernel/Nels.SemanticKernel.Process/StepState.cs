using Microsoft.SemanticKernel;
using System.Diagnostics;
using System.Text.Json.Serialization;

namespace Nels.SemanticKernel.Process;

public class StepState
{
    public string Id { get; set; }
    public KernelArguments Arguments { get; set; } = [];

    [JsonIgnore]
    public Stopwatch Stopwatch { get; set; } = new();
}
