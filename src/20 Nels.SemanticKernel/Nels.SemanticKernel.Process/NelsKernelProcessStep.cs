using Microsoft.AspNetCore.Http;
using Microsoft.SemanticKernel;
using Nels.SemanticKernel.Process.Consts;
using Nels.SemanticKernel.Process.Interfaces;
using Nels.SemanticKernel.Process.States;
using Nels.SemanticKernel.Process.Steps;
using System.Threading;
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
    protected Kernel _kernel;
    protected IHttpContextAccessor _httpContextAccessor;
    protected ProcessState _processState;
    protected IStepLog _stepLog;
    private readonly string ExecutedEvent = $"ExecutedEvent_{0}";
    private readonly List<LlmGetStreamingChatMessage> _chatMessages = [];

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
    public async ValueTask StepExecuteAsync(KernelProcessStepContext kernelProcessStepContext, Kernel kernel, CancellationToken cancellationToken)
    {
        _state.Stopwatch.Start();
        _processStepContext = kernelProcessStepContext;
        _cancellationToken = cancellationToken;
        _kernel = kernel;

        _httpContextAccessor = _kernel.GetRequiredService<IHttpContextAccessor>();

        if (_httpContextAccessor.HttpContext.Items.TryGetValue(nameof(ProcessState), out object? processState))
        {
            _processState = (ProcessState)processState;
            _stepLog = _processState.AgentChat.AddStepLog(Guid.Parse(_id));
        }

        if (await PreExecuteAsync(_cancellationToken) == false)
        {
            _state.Stopwatch.Stop();
            return;
        }

        _state.Stopwatch.Stop();
        if (_chatMessages?.Count > 0)
        {
            foreach (var chatMessage in _chatMessages)
            {
                if (chatMessage == null) continue;
                await chatMessage.Invoke(OnChatMessageAsync);
            }
            InitArguments();
        }
        _state.Stopwatch.Start();

        if (await ExecutionAsync(_cancellationToken) == false)
        {
            _state.Stopwatch.Stop();
            return;
        };

        await PostExecuteAsync(_cancellationToken);

        _state.Stopwatch.Stop();
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
        _stepLog.SetDuration(_state.Stopwatch.ElapsedMilliseconds);
        return default;
    }

    protected void InitArguments()
    {
        if (_state == null) throw new ArgumentNullException(nameof(IStepState));
        if (_state is IInputState state && state.Inputs != null)
        {
            _state.Arguments.Clear();
            _chatMessages.Clear();
            foreach (var input in state.Inputs)
            {
                var value = input.GetValue(_processState.Context);
                if (value is LlmGetStreamingChatMessage message)
                {
                    _chatMessages.Add(message);
                    continue;
                }
                _state.Arguments.Add(input.Name, value);
            }
        }
    }
}