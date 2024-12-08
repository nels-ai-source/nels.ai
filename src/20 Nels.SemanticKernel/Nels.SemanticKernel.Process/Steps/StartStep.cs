using Microsoft.SemanticKernel;
using Nels.SemanticKernel.Process.States;
using Nels.SemanticKernel.Process.Variables;
using System.Text.Json.Serialization;

namespace Nels.SemanticKernel.Process.Steps;

public class StartStep : NelsKernelProcessStep<StartStepState>
{
    [KernelFunction(StepTypeConst.Start)]
    public async ValueTask ExecuteAsync(KernelProcessStepContext context, CancellationToken cancellationToken)
    {
        await base.StepExecuteAsync(context, null, cancellationToken);
    }

    public override ValueTask PostExecuteAsync(CancellationToken cancellationToken)
    {
        var output = _state.Outputs.FirstOrDefault();
        if (output == null) return base.PostExecuteAsync(cancellationToken);

        if (_state.Context.TryGetValue(_id, out var values) && values is Dictionary<string, object> keyValues)
        {
            keyValues[output.Name] = _state.UserInput;
        }
        _state.Context.Add(_id, new Dictionary<string, object> { { output.Name, _state.UserInput } });

        return base.PostExecuteAsync(cancellationToken);
    }
}
public class StartStepState : StepState, IOutputState
{
    [JsonPropertyName("userInput")]
    public string UserInput { get; set; }

    [JsonPropertyName("outputs")]
    public List<Variables.OutputVariable> Outputs { get; set; } = [new Variables.OutputVariable { Name = StepConst.DefaultOutput, Type = VariableTypeConst.String }];
}