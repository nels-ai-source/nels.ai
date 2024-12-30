using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
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
using System.Text.Json;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Localization;
using Volo.Abp.Uow;

namespace Nels.Aigc.Services;


[Route(AigcRemoteServiceConsts.agentRoute)]
public class AgentAppService : RouteCrudGetAllAppService<Agent, AgentDto, Guid>
{
    private readonly IStreamResponse _streamResponse;
    private readonly IProceessSerializer _proceessSerializer;

    private readonly IRepository<AgentPresetQuestions, Guid> _presetQuestionsRepository;
    private readonly IRepository<AgentMetadata, Guid> _metadataRepository;
    private readonly IRepository<AgentConversation, Guid> _agentConversationRepository;
    private readonly IRepository<AgentChat, Guid> _agentChatRepository;
    private readonly IRepository<AgentMessage, Guid> _agentMessageRepository;

    private readonly AgentChatDomainService _agentChatDomainService;
    private readonly Kernel _kernel;

    public AgentAppService(IRepository<Agent, Guid> repository,
        IRepository<AgentPresetQuestions, Guid> presetQuestionsRepository,
        IRepository<AgentMetadata, Guid> metadataRepository,
        IRepository<AgentConversation, Guid> agentConversationRepository,
        IRepository<AgentMessage, Guid> agentMessageRepository,
        IRepository<AgentChat, Guid> agentChatRepository,
        ILanguageProvider languageProvider,
        IStreamResponse streamResponse,
        IOptions<AbpLocalizationOptions> localizationOptions,
        IProceessSerializer proceessSerializer,
        AgentChatDomainService agentChatDomainService,
        Kernel kernel) : base(repository)
    {
        CreatePolicyName = AigcPermissions.Agent.Create;
        UpdatePolicyName = AigcPermissions.Agent.Update;
        DeletePolicyName = AigcPermissions.Agent.Delete;
        GetPolicyName = AigcPermissions.Agent.GetList;
        GetListPolicyName = AigcPermissions.Agent.GetList;

        _presetQuestionsRepository = presetQuestionsRepository;
        _metadataRepository = metadataRepository;
        _agentConversationRepository = agentConversationRepository;
        _agentChatRepository = agentChatRepository;
        _agentMessageRepository = agentMessageRepository;

        _agentChatDomainService = agentChatDomainService;

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
    public virtual async Task AgentStartAsync(StartRequest request)
    {
        var agent = await GetAsync(request.AgentId) ?? throw new Exception();
        var agentConversation = request.AgentConversationId == null
            ? new AgentConversation(GuidGenerator.Create()) : await _agentConversationRepository.GetAsync(x => x.Id == request.AgentConversationId.Value);
        agentConversation.AgentId = agent.Id;

        var processState = AgentStartProcess(request, agentConversation);

        if (agent.AgentType == AgentType.Llm)
        {
            await LlmAgentStartAsync(request, agent, agentConversation);
        }
        else if (agent.AgentType == AgentType.Workflow)
        {
            await WorkflowAgentStartAsync(request, agent, agentConversation);
        }

        var agentChat = (processState.AgentChat as AgentChat) ?? throw new Exception();

        agentConversation.SetTitle(string.IsNullOrWhiteSpace(agentConversation.Title) ? agentChat.Question : agentConversation.Title);
        await _agentChatDomainService.InsertAgentChatAsync(agentChat);

        agentConversation = request.AgentConversationId == null ? await _agentConversationRepository.InsertAsync(agentConversation) : await _agentConversationRepository.UpdateAsync(agentConversation);
        if (request.Streaming)
        {
            await _streamResponse.WriteDataAsync(ProcessEventType.Down, new { ConversationId = agentConversation.Id, ChatId = agentChat.Id });
        }
    }

    private ProcessState AgentStartProcess(StartRequest request, AgentConversation agentConversation)
    {
        var processState = new ProcessState
        {
            AgentId = agentConversation.AgentId,
            AgentConversationId = agentConversation.Id,
            AgentChat = new AgentChat(GuidGenerator.Create(), agentConversation.AgentId, agentConversation.Id)
        };
        var httpContextAccessor = _kernel.GetRequiredService<IHttpContextAccessor>();
        if (httpContextAccessor.HttpContext.Items.ContainsKey(nameof(processState)))
        {
            httpContextAccessor.HttpContext.Items[nameof(processState)] = processState;
        }
        else
        {
            httpContextAccessor.HttpContext.Items.Add(nameof(ProcessState), processState);
        }
        if (request.Streaming)
        {
            _streamResponse.EnableStream();
        }
        return processState;
    }

    private async Task LlmAgentStartAsync(StartRequest request, AgentDto agent, AgentConversation agentConversation)
    {
        KernelProcess kernelProcess = BuilderLlmKernelProcess(request, agent);

        await kernelProcess.StartAsync(_kernel, new KernelProcessEvent { Id = StepEvent.StartProcessEvent, Data = request });

    }

    private KernelProcess BuilderLlmKernelProcess(StartRequest request, AgentDto agent)
    {
        var agentLlmState = string.IsNullOrWhiteSpace(agent.States) ? new AgentLlmStateDto() : JsonSerializer.Deserialize<AgentLlmStateDto>(agent.States) ?? throw new Exception();

        ProcessBuilder process = new(agent.Id.ToString());
        var startStep = process.AddStepFromType<StartStep>(nameof(StartStep));
        agentLlmState.LlmStepState.ChatMessages.Add(new MessageContent(AuthorRole.User.Label, request.UserInput));

        var llmStep = process.AddStepFromType<LlmStep, LlmStepState>(agentLlmState.LlmStepState, nameof(LlmStep));

        agentLlmState.MessageStepState.Inputs.Add(new SemanticKernel.Process.Variables.InputVariable
        {
            Name = StepConst.DefaultOutput,
            Type = VariableTypeConst.String,
            Value = new VariableValue
            {
                Type = VariableValueTypeConst.Ref,
                Content = StepConst.DefaultOutput,
                RefKey = llmStep.Id,

            }
        });
        agentLlmState.MessageStepState.Template = "{{" + StepConst.DefaultOutput + "}}";

        var messageStep = process.AddStepFromType<MessageStep, MessageStepState>(agentLlmState.MessageStepState, nameof(MessageStep));

        process.OnInputEvent(StepEvent.StartProcessEvent)
            .SendEventTo(new ProcessFunctionTargetBuilder(startStep, parameterName: "request"));

        startStep.OnFunctionResult()
            .SendEventTo(new ProcessFunctionTargetBuilder(llmStep, functionName: StepTypeConst.Llm));

        llmStep.OnFunctionResult()
             .SendEventTo(new ProcessFunctionTargetBuilder(messageStep, functionName: StepTypeConst.Message));

        return process.Build();
    }

    private async Task WorkflowAgentStartAsync(StartRequest request, AgentDto agent, AgentConversation agentConversation)
    {
        await Task.CompletedTask;
    }

    protected override Task UpdateInputMapToEntityAsync(AgentDto updateInput, Agent entity)
    {
        var index = 0;
        foreach (var item in updateInput.PresetQuestions)
        {
            item.Id = item.Id == Guid.Empty ? GuidGenerator.Create() : item.Id;
            item.Index = index++;
        }
        if (string.IsNullOrWhiteSpace(updateInput.Steps) == false || string.IsNullOrWhiteSpace(updateInput.States) == false)
        {
            entity.AddOrUpdateMetadata(GuidGenerator.Create(), updateInput.Steps, updateInput.States);
        }
        return base.UpdateInputMapToEntityAsync(updateInput, entity);
    }

    protected override async Task<Agent> GetEntityByIdAsync(Guid id)
    {
        var entity = await Repository.GetAsync(id);
        entity.PresetQuestions = await _presetQuestionsRepository.GetListAsync(x => x.AgentId == id);
        entity.Metadata = await _metadataRepository.FirstOrDefaultAsync(x => x.AgentId == id);

        entity.PresetQuestions = [.. entity.PresetQuestions.OrderBy(x => x.Index)];
        return entity;
    }

    [UnitOfWork]
    protected override async Task<Agent> UpdateAsync(Agent entity)
    {
        await _presetQuestionsRepository.DeleteAsync(x => x.AgentId == entity.Id);
        if (entity.PresetQuestions.Count != 0)
        {
            await _presetQuestionsRepository.InsertManyAsync(entity.PresetQuestions);
        }
        if (entity.Metadata != null)
        {
            if (await _metadataRepository.AnyAsync(x => x.Id == entity.Metadata.Id))
            {
                await _metadataRepository.UpdateAsync(entity.Metadata);
            }
            else
            {
                await _metadataRepository.InsertAsync(entity.Metadata);
            }
        }
        return await base.UpdateAsync(entity);
    }

    #region conversation
    [HttpPost]
    [Route("[action]")]
    public virtual async Task<AgentDto> GetAgentConversationsAsync(Guid agentId)
    {
        var entity = await Repository.GetAsync(agentId) ?? throw new BusinessException("not found");
        var entities = await _agentConversationRepository.GetListAsync(x => x.AgentId == agentId && x.CreatorId == CurrentUser.Id);

        var dto = Map<Agent, AgentDto>(entity);
        dto.Conversations = MapList<AgentConversation, AgentConversationDto>([.. entities.OrderByDescending(x => x.CreationTime)]);

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
