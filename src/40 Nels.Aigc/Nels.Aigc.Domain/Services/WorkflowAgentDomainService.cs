using Microsoft.AspNetCore.Http;
using Microsoft.SemanticKernel;
using Nels.Abp.Ddd.Domain.Services;
using Nels.Aigc.Entities;
using Nels.SemanticKernel.Interfaces;
using Nels.SemanticKernel.Process;
using System;
using System.Threading.Tasks;
using Volo.Abp.Guids;

namespace Nels.Aigc.Services;

public class WorkflowAgentDomainService : DomainService
{
    public async Task WorkflowAgentStartAsync(StartRequest request, AgentEntity agent)
    {
        await Task.CompletedTask;
    }
    //    var agentChat = (processState.AgentChat as AgentChat) ?? throw new Exception();



    //    agentConversation = request.AgentConversationId == null ? await _agentConversationRepository.InsertAsync(agentConversation) : await _agentConversationRepository.UpdateAsync(agentConversation);
    //        if (request.Streaming)
    //        {
    //            await _streamResponse.WriteDataAsync(ProcessEventType.Down, new { ConversationId = agentConversation.Id, ChatId = agentChat.Id
    //});
    //        }
    //    }

    //    private ProcessState AgentStartProcess(StartRequest request, AgentConversation agentConversation)
    //{
    //    var processState = new ProcessState
    //    {
    //        AgentId = agentConversation.AgentId,
    //        AgentConversationId = agentConversation.Id,
    //        AgentChat = new AgentChat(GuidGenerator.Create(), agentConversation.AgentId, agentConversation.Id)
    //    };
    //    processState.AgentChat.Question = request.UserInput;
    //    var httpContextAccessor = _kernel.GetRequiredService<IHttpContextAccessor>();
    //    if (httpContextAccessor.HttpContext.Items.ContainsKey(nameof(processState)))
    //    {
    //        httpContextAccessor.HttpContext.Items[nameof(processState)] = processState;
    //    }
    //    else
    //    {
    //        httpContextAccessor.HttpContext.Items.Add(nameof(ProcessState), processState);
    //    }
    //    if (request.Streaming)
    //    {
    //        _streamResponse.EnableStream();
    //    }
    //    return processState;
    //}
    //private KernelProcess BuilderLlmKernelProcess(StartRequest request, AgentDto agent)
    //{
    //    var agentLlmState = string.IsNullOrWhiteSpace(agent.States) ? new AgentLlmStateDto() : JsonSerializer.Deserialize<AgentLlmStateDto>(agent.States) ?? throw new Exception();

    //    ProcessBuilder process = new(agent.Id.ToString());
    //    var startStep = process.AddStepFromType<StartStep>(nameof(StartStep));
    //    agentLlmState.LlmStepState.ChatMessages.Add(new MessageContent(AuthorRole.User.Label, request.UserInput));

    //    var llmStep = process.AddStepFromType<LlmStep, LlmStepState>(agentLlmState.LlmStepState, nameof(LlmStep));

    //    agentLlmState.MessageStepState.Inputs.Add(new SemanticKernel.Process.Variables.InputVariable
    //    {
    //        Name = StepConst.DefaultOutput,
    //        Type = VariableTypeConst.String,
    //        Value = new VariableValue
    //        {
    //            Type = VariableValueTypeConst.Ref,
    //            Content = StepConst.DefaultOutput,
    //            RefKey = llmStep.Id,

    //        }
    //    });
    //    agentLlmState.MessageStepState.Template = "{{" + StepConst.DefaultOutput + "}}";

    //    var messageStep = process.AddStepFromType<MessageStep, MessageStepState>(agentLlmState.MessageStepState, nameof(MessageStep));

    //    process.OnInputEvent(StepEvent.StartProcessEvent)
    //        .SendEventTo(new ProcessFunctionTargetBuilder(startStep, parameterName: "request"));

    //    startStep.OnFunctionResult()
    //        .SendEventTo(new ProcessFunctionTargetBuilder(llmStep, functionName: StepTypeConst.Llm));

    //    llmStep.OnFunctionResult()
    //         .SendEventTo(new ProcessFunctionTargetBuilder(messageStep, functionName: StepTypeConst.Message));

    //    return process.Build();
    //}
}
