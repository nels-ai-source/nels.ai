using Microsoft.SemanticKernel;
using Nels.SemanticKernel.Process.Consts;
using Nels.SemanticKernel.Process.Extensions;
using Nels.SemanticKernel.Process.States;
using Nels.SemanticKernel.Process.Variables;
using System.Text.Json.Serialization;

namespace Nels.SemanticKernel.Process.Steps;

public class StartStep : NelsKernelProcessStep<StartStepState>
{
    private StartRequest _request;
    [KernelFunction(StepTypeConst.Start)]
    public async ValueTask ExecuteAsync(KernelProcessStepContext context, StartRequest request, Kernel kernel, CancellationToken cancellationToken)
    {
        _request = request;
        await base.StepExecuteAsync(context, kernel, cancellationToken);
    }

    public override ValueTask PostExecuteAsync(CancellationToken cancellationToken)
    {
        var output = _state.Outputs.FirstOrDefault();
        if (output == null) return base.PostExecuteAsync(cancellationToken);

        _processState.Context.AddDefaultOutput(_id, _request.UserInput);
        _processState.AgentChat.AddMessage(MessageRoleConsts.User, _request.UserInput);

        return base.PostExecuteAsync(cancellationToken);
    }
}
public class StartStepState : StepState, IOutputState
{
    [JsonPropertyName("outputs")]
    public List<Variables.OutputVariable> Outputs { get; set; } = [new Variables.OutputVariable { Name = StepConst.DefaultOutput, Type = VariableTypeConst.String }];
}