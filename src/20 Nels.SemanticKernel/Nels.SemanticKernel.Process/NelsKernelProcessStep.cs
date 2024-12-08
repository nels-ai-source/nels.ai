using Microsoft.SemanticKernel;
using Nels.SemanticKernel.Interfaces;
using Nels.SemanticKernel.Process.States;
using Nels.SemanticKernel.Process.Steps;
using static Nels.SemanticKernel.Process.Steps.LlmStep;

namespace Nels.SemanticKernel.Process;


/// <summary>
/// Process Step. Derive from this class to create a new Step with user-defined state of type TState for a Process.
/// </summary>
/// <typeparam name="TState">An instance of TState used for user-defined state.</typeparam>
public class NelsKernelProcessStep<IStepState> : KernelProcessStep<IStepState> where IStepState : StepState, new()
{
    protected IStepState _state = new();
    protected string _id;
    protected string _name;
    protected KernelProcessStepContext _processStepContext;

    private readonly string ExecutedEvent = $"ExecutedEvent_{0}";


    /// <inheritdoc/>
    public override ValueTask ActivateAsync(KernelProcessStepState<IStepState> state)
    {
        _id = state.Id ?? new Guid().ToString();
        _name = state.Name ?? string.Empty;
        _state = state.State ?? (IStepState)new StepState();
        return base.ActivateAsync(state);
    }
    protected CancellationToken _cancellationToken;
    public override ValueTask ActivateAsync(KernelProcessStepState state)
    {
        return base.ActivateAsync(state);
    }
    public async ValueTask StepExecuteAsync(KernelProcessStepContext kernelProcessStepContext, Dictionary<string, object>? content, CancellationToken cancellationToken)
    {
        _processStepContext = kernelProcessStepContext;
        _cancellationToken = cancellationToken;
        _state.Context = content ?? [];

        if (await PreExecuteAsync(_cancellationToken) == false) return;

        var chatMessages = _state.Arguments.Where(x => x.Value is LlmGetStreamingChatMessage message).Select(x => x.Value as LlmGetStreamingChatMessage)
            .Distinct().ToList();
        if (chatMessages?.Count > 0)
        {
            foreach (var chatMessage in chatMessages)
            {
                if (chatMessage == null) continue;
                await chatMessage.Invoke(OnChatMessageAsync);
            }
            InitArguments();
        }

        if (await ExecutionAsync(_cancellationToken) == false)
        {
            await _processStepContext.EmitEventAsync(StepEvent.ExecutedEvent, _state.Context);
            return;
        };

        await PostExecuteAsync(_cancellationToken);
        await _processStepContext.EmitEventAsync(StepEvent.ExecutedEvent, _state.Context);
    }

    public virtual async ValueTask OnChatMessageAsync(LlmChatMessageEventData data)
    {
        await Task.CompletedTask;
    }
    public virtual async ValueTask<bool> PreExecuteAsync(CancellationToken cancellationToken)
    {
        InitArguments();
        return await Task.FromResult(true);
    }
    public virtual async ValueTask<bool> ExecutionAsync(CancellationToken cancellationToken)
    {
        return await Task.FromResult(true);
    }
    public virtual ValueTask PostExecuteAsync(CancellationToken cancellationToken)
    {
        return default;
    }

    protected void InitArguments()
    {
        if (_state == null) throw new ArgumentNullException(nameof(IStepState));
        if (_state is IInputState state && state.Inputs != null)
        {
            _state.Arguments.Clear();
            foreach (var input in state.Inputs)
            {
                _state.Arguments.Add(input.Name, input.GetValue(_state));
            }
        }
    }
}