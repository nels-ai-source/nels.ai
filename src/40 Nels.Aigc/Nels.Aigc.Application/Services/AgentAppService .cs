using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.SemanticKernel;
using Nels.Abp.Ddd.Application.Services;
using Nels.Aigc.Dtos;
using Nels.Aigc.Entities;
using Nels.Aigc.Enums;
using Nels.Aigc.Permissions;
using Nels.SemanticKernel.Interfaces;
using Nels.SemanticKernel.Process;
using Nels.SemanticKernel.Process.Consts;
using Nels.SemanticKernel.Process.Interfaces;
using Nels.SemanticKernel.Process.Steps;
using Nels.SemanticKernel.Process.Variables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Localization;
using Volo.Abp.Uow;

namespace Nels.Aigc.Services;


[Route(AigcRemoteServiceConsts.agentRoute)]
public class AgentAppService : RouteCrudGetAllAppService<AgentEntity, AgentDto, Guid>
{
    private readonly IStreamResponse _streamResponse;
    private readonly IProceessSerializer _proceessSerializer;

    private readonly IRepository<AgentPresetQuestions, Guid> _presetQuestionsRepository;
    private readonly IRepository<LlmAgentMetadata, Guid> _llmMetadataRepository;
    private readonly IRepository<WorkflowAgentMetadata, Guid> _wfMetadataRepository;
    private readonly IRepository<AgentConversationEntity, Guid> _agentConversationRepository;
    private readonly IRepository<AgentChat, Guid> _agentChatRepository;
    private readonly IRepository<AgentMessage, Guid> _agentMessageRepository;

    private readonly AgentChatDomainService _agentChatDomainService;
    private readonly LlmAgentDomainService _llmAgentDomainService;
    private readonly WorkflowAgentDomainService _workflowAgentDomainService;
    private readonly Kernel _kernel;

    public AgentAppService(IRepository<AgentEntity, Guid> repository,
        IRepository<AgentPresetQuestions, Guid> presetQuestionsRepository,
        IRepository<LlmAgentMetadata, Guid> llmMetadataRepository,
        IRepository<WorkflowAgentMetadata, Guid> wfMetadataRepository,
        IRepository<AgentConversationEntity, Guid> agentConversationRepository,
        IRepository<AgentMessage, Guid> agentMessageRepository,
        IRepository<AgentChat, Guid> agentChatRepository,
        ILanguageProvider languageProvider,
        IStreamResponse streamResponse,
        IOptions<AbpLocalizationOptions> localizationOptions,
        IProceessSerializer proceessSerializer,
        AgentChatDomainService agentChatDomainService,
        LlmAgentDomainService llmAgentDomainService,
        WorkflowAgentDomainService workflowAgentDomainService,
    Kernel kernel) : base(repository)
    {
        CreatePolicyName = AigcPermissions.Agent.Create;
        UpdatePolicyName = AigcPermissions.Agent.Update;
        DeletePolicyName = AigcPermissions.Agent.Delete;
        GetPolicyName = AigcPermissions.Agent.GetList;
        GetListPolicyName = AigcPermissions.Agent.GetList;

        _presetQuestionsRepository = presetQuestionsRepository;
        _llmMetadataRepository = llmMetadataRepository;
        _wfMetadataRepository = wfMetadataRepository;
        _agentConversationRepository = agentConversationRepository;
        _agentChatRepository = agentChatRepository;
        _agentMessageRepository = agentMessageRepository;

        _agentChatDomainService = agentChatDomainService;
        _llmAgentDomainService = llmAgentDomainService;
        _workflowAgentDomainService = workflowAgentDomainService;

        _kernel = kernel;
        _streamResponse = streamResponse;
        _proceessSerializer = proceessSerializer;
    }


    [HttpPost]
    [Route("[action]")]
    public virtual async Task testAsync()
    {
        ProcessBuilder process = new("AccountOpeningProcess");
        var startStep = process.AddStepFromType<StartStep>(nameof(StartStep));
        var llmStep_0 = process.AddStepFromType<LlmStep, LlmStepState>(new LlmStepState
        {
            ChatMessages = [new MessageContent { Role = "user", Content = "写一首七言律诗" }]
        }, "llmStep_0");
        var llmStep_1 = process.AddStepFromType<LlmStep, LlmStepState>(new LlmStepState
        {
            ChatMessages = [new MessageContent { Role = "user", Content = "写一个递归算法" }]
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

        var con = await kernelProcess.StartAsync(_kernel, new KernelProcessEvent { Id = "StartProcess", Data = new StartStepState() });
        var res = await con.GetStateAsync();
        var json = _proceessSerializer.Serialize(res);
        var http = _kernel.GetRequiredService<IHttpContextAccessor>();
        var item = http.HttpContext.Items;
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

        var kernelProcess = _proceessSerializer.Deserialize(jsonProcess);

        var processStateInfo = kernelProcess.ToProcessStateMetadata();

        //  var content = JsonSerializer.Serialize<KernelProcessStepStateMetadata>(processStateInfo, s_jsonOptions);

        var con = await kernelProcess.StartAsync(_kernel, new KernelProcessEvent { Id = "StartProcess", Data = new StartStepState() });

        var res = await con.GetStateAsync();
    }

    [HttpPost]
    [Route("[action]")]
    public virtual async Task AgentStartAsync(StartRequest request, CancellationToken cancellation = default)
    {
        var agent = await GetEntityByIdAsync(request.AgentId) ?? throw new Exception();

        if (agent.AgentType == AgentType.Llm)
        {
            await _llmAgentDomainService.InvokeStreamingAsync(request, agent, cancellation);
        }
        else if (agent.AgentType == AgentType.Workflow)
        {
            await _workflowAgentDomainService.WorkflowAgentStartAsync(request, agent);
        }
    }


    protected override Task UpdateInputMapToEntityAsync(AgentDto updateInput, AgentEntity entity)
    {
        var index = 0;
        foreach (var item in updateInput.PresetQuestions)
        {
            item.Id = item.Id == Guid.Empty ? GuidGenerator.Create() : item.Id;
            item.AgentId = entity.Id;
            item.Index = index++;
        }
        return base.UpdateInputMapToEntityAsync(updateInput, entity);
    }

    protected override async Task<AgentEntity> GetEntityByIdAsync(Guid id)
    {
        var entity = await Repository.GetAsync(id);
        entity.PresetQuestions = await _presetQuestionsRepository.GetListAsync(x => x.AgentId == id);
        if (entity.AgentType == AgentType.Llm)
        {
            entity.Metadata = await _llmMetadataRepository.FirstOrDefaultAsync(x => x.AgentId == id);
        }
        else if (entity.AgentType == AgentType.Workflow)
        {
            entity.Metadata = await _wfMetadataRepository.FirstOrDefaultAsync(x => x.AgentId == id);
        }

        entity.PresetQuestions = [.. entity.PresetQuestions.OrderBy(x => x.Index)];
        return entity;
    }

    protected override async Task ProcessUpdate(AgentDto input, AgentEntity entity)
    {
        if (input is LlmAgentDto llmAgent)
        {
            if (entity.Metadata == null)
            {
                entity.AddOrUpdateLlmWorkflowMetadata(GuidGenerator.Create(), llmAgent.Prompt, llmAgent.ChatReducerCount, llmAgent.ToolAutoInvoke);
                return;
            }
            LlmAgentMetadata? metadata = await _llmMetadataRepository.FirstOrDefaultAsync(x => x.AgentId == entity.Id);
            if (metadata == null) return;

            ObjectMapper.Map(llmAgent, metadata);
            entity.Metadata = metadata;
        }

    }
    [UnitOfWork]
    protected override async Task<AgentEntity> UpdateAsync(AgentEntity entity)
    {
        await _presetQuestionsRepository.DeleteAsync(x => x.AgentId == entity.Id);
        if (entity.PresetQuestions.Count != 0)
        {
            await _presetQuestionsRepository.InsertManyAsync(entity.PresetQuestions);
        }
        if (entity.Metadata != null)
        {
            if (entity.Metadata is LlmAgentMetadata llmMetadata)
            {
                if (await _llmMetadataRepository.AnyAsync(x => x.Id == llmMetadata.Id))
                {
                    await _llmMetadataRepository.UpdateAsync(llmMetadata);
                }
                else
                {
                    await _llmMetadataRepository.InsertAsync(llmMetadata);
                }
            }
            else if (entity.Metadata is WorkflowAgentMetadata wfMetadata)
            {
                if (await _wfMetadataRepository.AnyAsync(x => x.Id == wfMetadata.Id))
                {
                    await _wfMetadataRepository.UpdateAsync(wfMetadata);
                }
                else
                {
                    await _wfMetadataRepository.InsertAsync(wfMetadata);
                }
            }
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

    [HttpPost]
    [Route("[action]")]
    public virtual async Task<LlmAgentDto> GetLlmAgentAsync(Guid id)
    {
        var entity = await GetEntityByIdAsync(id);
        var dto = ObjectMapper.Map<AgentEntity, LlmAgentDto>(entity);
        if (entity.Metadata != null && entity.Metadata is LlmAgentMetadata metadata)
        {
            ObjectMapper.Map(metadata, dto);
        }

        return dto;
    }
    #endregion

    #region conversation
    [HttpPost]
    [Route("[action]")]
    public virtual async Task<AgentDto> GetAgentConversationsAsync(Guid agentId)
    {
        var entity = await Repository.GetAsync(agentId) ?? throw new BusinessException("not found");
        var entities = await _agentConversationRepository.GetListAsync(x => x.AgentId == agentId && x.CreatorId == CurrentUser.Id);

        var dto = Map<AgentEntity, AgentDto>(entity);
        dto.Conversations = MapList<AgentConversationEntity, AgentConversationDto>([.. entities.OrderByDescending(x => x.CreationTime)]);

        return dto;
    }

    [HttpPost]
    [Route("[action]")]
    public virtual async Task UpdateConversationTitleAsync(AgentConversationDto conversationDto)
    {
        var entity = await _agentConversationRepository.GetAsync(conversationDto.Id);
        entity.SetTitle(conversationDto.Title);

        await _agentConversationRepository.UpdateAsync(entity, true);
    }

    [HttpPost]
    [Route("[action]")]
    public virtual async Task DeleteConversationAsync(Guid conversationId)
    {
        await _agentConversationRepository.DeleteAsync(x => x.Id == conversationId);
        await _agentChatRepository.DeleteAsync(x => x.AgentConversationId == conversationId);
        await _agentMessageRepository.DeleteAsync(x => x.AgentConversationId == conversationId);
    }
    #endregion

    #region chat
    [HttpPost]
    [Route("[action]")]
    public virtual async Task RegenChatAsync(Guid chatId)
    {

        var entities = await _agentMessageRepository.GetListAsync(x => x.AgentChatId == chatId && x.CreatorId == CurrentUser.Id);
        if (entities == null) return;

        var userMessage = entities.FirstOrDefault(x => x.Role == MessageRoleConsts.User);
        if (userMessage == null) return;

        await _agentMessageRepository.DeleteManyAsync(entities);

        StartRequest request = new()
        {
            AgentId = userMessage.AgentId,
            AgentConversationId = userMessage.AgentConversationId,
            Streaming = true,
            UserInput = userMessage.Content,
        };

        await AgentStartAsync(request);
    }
    #endregion

    #region message
    [HttpPost]
    [Route("[action]")]
    public virtual async Task DeleteMessageAsync(Guid messageId)
    {
        await _agentMessageRepository.DeleteAsync(x => x.Id == messageId && x.CreatorId == CurrentUser.Id);
    }

    [HttpPost]
    [Route("[action]")]
    public virtual async Task<List<AgentMessageDto>> GetAgentMessagesAsync(Guid agentConversationId)
    {
        var entities = await _agentMessageRepository.GetListAsync(x => x.AgentConversationId == agentConversationId && x.CreatorId == CurrentUser.Id);
        return MapList<AgentMessage, AgentMessageDto>([.. entities.OrderBy(x => x.CreationTime).ThenBy(x => x.Index)]);
    }
    #endregion
}
