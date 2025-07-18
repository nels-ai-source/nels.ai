using AutoMapper.Configuration.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Process.Models;
using Nels.Abp.Ddd.Application.Services;
using Nels.Aigc.Dtos;
using Nels.Aigc.Entities;
using Nels.Aigc.Enums;
using Nels.Aigc.Permissions;
using Nels.SemanticKernel.Core.Enums;
using Nels.SemanticKernel.Interfaces;
using Nels.SemanticKernel.Process;
using Nels.SemanticKernel.Process.Consts;
using Nels.SemanticKernel.Process.Interfaces;
using Nels.SemanticKernel.Process.Steps;
using Nels.SemanticKernel.Process.Variables;
using Nels.SemanticKernel.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Localization;
using Volo.Abp.Uow;

namespace Nels.Aigc.Services;


[Route(AigcRemoteServiceConsts.agentRoute)]
public class AgentAppService : RouteCrudGetAllAppService<Agent, AgentDto, Guid>, IAgentService
{
    private readonly IProceessSerializer proceessSerializer;

    private readonly IRepository<AgentPresetQuestions, Guid> presetQuestionsRepository;
    private readonly IRepository<Conversation, Guid> agentConversationRepository;
    private readonly IRepository<Chat, Guid> agentChatRepository;
    private readonly IRepository<ChatMessage, Guid> agentMessageRepository;

    private readonly Kernel kernel;

    public AgentAppService(IRepository<Agent, Guid> repository,
        IRepository<AgentPresetQuestions, Guid> presetQuestionsRepository,
        IRepository<Conversation, Guid> agentConversationRepository,
        IRepository<ChatMessage, Guid> agentMessageRepository,
        IRepository<Chat, Guid> agentChatRepository,
        ILanguageProvider languageProvider,
        IStreamResponse streamResponse,
        IOptions<AbpLocalizationOptions> localizationOptions,
        IProceessSerializer proceessSerializer,
        Lazy<AgentActuator> agentActuator,
        ChatAggregateService agentChatDomainService,
        LlmAgentDomainService llmAgentDomainService,
        WorkflowAgentDomainService workflowAgentDomainService,
    Kernel kernel) : base(repository)
    {
        //CreatePolicyName = AigcPermissions.Agent.Create;
        //UpdatePolicyName = AigcPermissions.Agent.Update;
        //DeletePolicyName = AigcPermissions.Agent.Delete;
        //GetPolicyName = AigcPermissions.Agent.GetList;
        //GetListPolicyName = AigcPermissions.Agent.GetList;

        this.presetQuestionsRepository = presetQuestionsRepository;
        this.agentConversationRepository = agentConversationRepository;
        this.agentChatRepository = agentChatRepository;
        this.agentMessageRepository = agentMessageRepository;

        this.kernel = kernel;
        this.proceessSerializer = proceessSerializer;
    }


    [HttpPost]
    [Route("[action]")]
    public virtual async Task testAsync()
    {
        ProcessBuilder process = new("AccountOpeningProcess");
        var startStep = process.AddStepFromType<StartStep>(nameof(StartStep));
        var llmStep_0 = process.AddStepFromType<LlmStep, LlmStepState>(new LlmStepState
        {
            ChatMessages = [new SemanticKernel.Process.Steps.MessageContent { Role = "user", Content = "写一首七言律诗" }]
        }, "llmStep_0");
        var llmStep_1 = process.AddStepFromType<LlmStep, LlmStepState>(new LlmStepState
        {
            ChatMessages = [new SemanticKernel.Process.Steps.MessageContent { Role = "user", Content = "写一个递归算法" }]
        }, "llmStep_1");


        var messageStep = process.AddStepFromType<MessageStep, MessageStepState>(new MessageStepState
        {
            Template = "message1:{{llmStep_0_output}}",
            Inputs = [
                new SemanticKernel.Process.Variables.InputVariable
                {
            Name = StepConst.DefaultOutput+"0",
            Type = VariableTypeConst.String,
            Value = new VariableValue
            {
                Type=VariableValueTypeConst.Ref,
                Content = StepConst.DefaultOutput,
                RefKey = llmStep_0.Id,
            }
        },        new SemanticKernel.Process.Variables.InputVariable
        {
            Name = StepConst.DefaultOutput+"1",
            Type = VariableTypeConst.String,
            Value = new VariableValue
            {
                 Type=VariableValueTypeConst.Ref,
                Content = StepConst.DefaultOutput,
                RefKey = llmStep_1.Id,
            }
        }
            ]
        }, nameof(MessageStep));

        process.OnInputEvent("StartProcess")
            .SendEventTo(new ProcessFunctionTargetBuilder(startStep));

        startStep.OnFunctionResult()
            .SendEventTo(new ProcessFunctionTargetBuilder(llmStep_0))
            .SendEventTo(new ProcessFunctionTargetBuilder(llmStep_1));

        llmStep_1.OnFunctionResult()
             .SendEventTo(new ProcessFunctionTargetBuilder(messageStep));

        KernelProcess kernelProcess = process.Build();

        var con = await kernelProcess.StartAsync(kernel, new KernelProcessEvent { Id = "StartProcess", Data = new StartStepState() });
        var res = await con.GetStateAsync();
        var json = proceessSerializer.Serialize(res);

    }

    [HttpPost]
    [Route("[action]")]
    public virtual async Task ExecuteProcessAsync()
    {
        //ProcessBuilder process = new("AccountOpeningProcess");
        //var startStep = process.AddStepFromType<StartStep>(nameof(StartStep));
        //var llmStep = process.AddStepFromType<LlmStep>(nameof(LlmStep));
        //var messageStep = process.AddStepFromType<MessageStep>(nameof(MessageStep));

        //process.OnInputEvent("StartProcess")
        //    .SendEventTo(new ProcessFunctionTargetBuilder(startStep));

        //startStep.OnEvent(StepEvent.ExecutedEvent)
        //    .SendEventTo(new ProcessFunctionTargetBuilder(llmStep, parameterName: "content"));

        //llmStep.OnEvent(StepEvent.ExecutedEvent)
        //     .SendEventTo(new ProcessFunctionTargetBuilder(messageStep, parameterName: "content"));

        //KernelProcess kernelProcess = process.Build();
        //var daprProcess = DaprProcessInfo.FromKernelProcess(kernelProcess);
        //var json = JsonSerializer.Serialize(daprProcess, _serializerOptions);



        var jsonProcess = "{\"steps\":[{\"innerStepDotnetType\":\"Nels.SemanticKernel.Process.Steps.StartStep, Nels.SemanticKernel.Process, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\",\"state\":{\"$state-type\":\"StartStepState\",\"state\":{\"userInput\":null,\"outputs\":[{\"name\":\"output\",\"type\":\"String\",\"description\":null}],\"context\":{},\"arguments\":{},\"id\":null},\"id\":\"83aad4e91a064b91be96854f1f14e390\",\"name\":\"StartStep\",\"version\":\"v1\"},\"edges\":{\"StartStep_83aad4e91a064b91be96854f1f14e390.ExecutedEvent\":[{\"sourceStepId\":\"83aad4e91a064b91be96854f1f14e390\",\"outputTarget\":{\"stepId\":\"ed45567856ce4279ac9bb08a8fe4fe9f\",\"functionName\":\"llm\",\"parameterName\":\"content\",\"targetEventId\":null}}]}},{\"innerStepDotnetType\":\"Nels.SemanticKernel.Process.Steps.LlmStep, Nels.SemanticKernel.Process, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\",\"state\":{\"$state-type\":\"LlmStepState\",\"state\":{\"modelId\":null,\"extensionData\":{},\"inputs\":[],\"historyCount\":0,\"chatHistory\":[],\"chatMessages\":[],\"outputs\":[],\"response_format\":\"text\",\"context\":{},\"arguments\":{},\"id\":null},\"id\":\"ed45567856ce4279ac9bb08a8fe4fe9f\",\"name\":\"LlmStep\",\"version\":\"v1\"},\"edges\":{\"LlmStep_ed45567856ce4279ac9bb08a8fe4fe9f.ExecutedEvent\":[{\"sourceStepId\":\"ed45567856ce4279ac9bb08a8fe4fe9f\",\"outputTarget\":{\"stepId\":\"d153484a77e04edcaa7092db159c7650\",\"functionName\":\"message\",\"parameterName\":\"content\",\"targetEventId\":null}}]}},{\"innerStepDotnetType\":\"Nels.SemanticKernel.Process.Steps.MessageStep, Nels.SemanticKernel.Process, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\",\"state\":{\"$state-type\":\"MessageStepState\",\"state\":{\"inputs\":[],\"streamingOutput\":false,\"template\":null,\"template_format\":\"liquid\",\"context\":{},\"arguments\":{},\"id\":null},\"id\":\"d153484a77e04edcaa7092db159c7650\",\"name\":\"MessageStep\",\"version\":\"v1\"},\"edges\":{}}],\"innerStepDotnetType\":\"Microsoft.SemanticKernel.KernelProcess, Microsoft.SemanticKernel.Process.Abstractions, Version=1.29.0.0, Culture=neutral, PublicKeyToken=f300afd708cefcd3\",\"state\":{\"$state-type\":\"process\",\"id\":null,\"name\":\"AccountOpeningProcess\",\"version\":\"v1\"},\"edges\":{\"StartProcess\":[{\"sourceStepId\":\"9f55271850fe41e4bbadde3f6e2d69da\",\"outputTarget\":{\"stepId\":\"83aad4e91a064b91be96854f1f14e390\",\"functionName\":\"start\",\"parameterName\":null,\"targetEventId\":null}}]}}";

        var kernelProcess = proceessSerializer.Deserialize(jsonProcess);

        var processStateInfo = kernelProcess.ToProcessStateMetadata();

        var con = await kernelProcess.StartAsync(kernel, new KernelProcessEvent { Id = "StartProcess", Data = new StartStepState() });

        var res = await con.GetStateAsync();
    }




    protected override Task UpdateInputMapToEntityAsync(AgentDto updateInput, Agent entity)
    {
        var index = 0;
        foreach (var item in updateInput.Questions)
        {
            item.Id = item.Id == Guid.Empty ? GuidGenerator.Create() : item.Id;
            item.AgentId = entity.Id;
            item.Index = index++;
        }
        return base.UpdateInputMapToEntityAsync(updateInput, entity);
    }

    protected override async Task<Agent> GetEntityByIdAsync(Guid id)
    {
        var entity = await Repository.GetAsync(id);
        entity.Questions = await presetQuestionsRepository.GetListAsync(x => x.AgentId == id);
        entity.Questions = [.. entity.Questions.OrderBy(x => x.Index)];
        return entity;
    }


    [UnitOfWork]
    protected override async Task<Agent> UpdateAsync(Agent entity)
    {
        await presetQuestionsRepository.DeleteAsync(x => x.AgentId == entity.Id);
        if (entity.Questions.Count != 0)
        {
            await presetQuestionsRepository.InsertManyAsync(entity.Questions);
        }
        return await base.UpdateAsync(entity);
    }

    #region llmAgent
    [HttpPost]
    [Route("[action]")]
    public virtual async Task<LlmAgentDto> UpdateLlmAgentAsync(Guid id, LlmAgentDto input)
    {
        await base.UpdateAsync(id, input);
        return input;
    }

    #endregion

    #region conversation

    [HttpPost]
    [Route("[action]")]
    public virtual async Task UpdateConversationTitleAsync(ConversationDto conversationDto)
    {
        var entity = await agentConversationRepository.GetAsync(conversationDto.Id);
        entity.SetTitle(conversationDto.Title);

        await agentConversationRepository.UpdateAsync(entity, true);
    }

    [HttpPost]
    [Route("[action]")]
    public virtual async Task DeleteConversationAsync(Guid conversationId)
    {
        await agentConversationRepository.DeleteAsync(x => x.Id == conversationId);
        await agentChatRepository.DeleteAsync(x => x.ConversationId == conversationId);
        await agentMessageRepository.DeleteAsync(x => x.ConversationId == conversationId);
    }
    #endregion

    #region message
    [HttpPost]
    [Route("[action]")]
    public virtual async Task DeleteMessageAsync(Guid messageId)
    {
        await agentMessageRepository.DeleteAsync(x => x.Id == messageId && x.CreatorId == CurrentUser.Id);
    }

    [HttpPost]
    [Route("[action]")]
    public virtual async Task<List<ChatMessageDto>> GetAgentMessagesAsync(Guid agentConversationId)
    {
        var entities = await agentMessageRepository.GetListAsync(x => x.ConversationId == agentConversationId && x.CreatorId == CurrentUser.Id);
        return MapList<ChatMessage, ChatMessageDto>([.. entities.OrderBy(x => x.CreationTime).ThenBy(x => x.Index)]);
    }

    [RemoteService(IsEnabled = false)]
    public virtual async Task<IAgent> GetAgentAsync(Guid id)
    {
        return await this.GetAsync(id);
    }
    #endregion
}
