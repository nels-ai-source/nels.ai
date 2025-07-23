using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.SemanticKernel;
using Nels.Abp.Ddd.Application.Services;
using Nels.Aigc.Dtos;
using Nels.Aigc.Entities;
using Nels.SemanticKernel.Interfaces;
using Nels.SemanticKernel.Process;
using Nels.SemanticKernel.Process.Interfaces;
using Nels.SemanticKernel.Process.Steps;
using Nels.SemanticKernel.Process.Variables;
using Nels.SemanticKernel.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Localization;

namespace Nels.Aigc.Services;


[Route(AigcRemoteServiceConsts.agentRoute)]
public class AgentAppService : RouteCrudGetAllAppService<Agent, AgentDto, Guid, AgentGetListInputDto>, IAgentService
{
    private readonly IProceessSerializer proceessSerializer;

    private readonly IRepository<AgentPresetQuestions, Guid> presetQuestionsRepository;
    private readonly IRepository<AgentKnowledge, Guid> agentKnowledgeRepository;
    private readonly IRepository<AgentTool, Guid> agentToolRepository;
    private readonly IRepository<Conversation, Guid> agentConversationRepository;
    private readonly IRepository<Chat, Guid> agentChatRepository;
    private readonly IRepository<ChatMessage, Guid> agentMessageRepository;
    private readonly IRepository<Knowledge, Guid> knowledgeRepository;
    private readonly IRepository<Tool, Guid> toolRepository;

    private readonly Kernel kernel;

    public AgentAppService(IRepository<Agent, Guid> repository,
        IRepository<AgentPresetQuestions, Guid> presetQuestionsRepository,
        IRepository<AgentKnowledge, Guid> agentKnowledgeRepository,
        IRepository<AgentTool, Guid> agentToolRepository,
        IRepository<Conversation, Guid> agentConversationRepository,
        IRepository<ChatMessage, Guid> agentMessageRepository,
        IRepository<Chat, Guid> agentChatRepository,
        IRepository<Knowledge, Guid> knowledgeRepository,
        IRepository<Tool, Guid> toolRepository,
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
        this.agentKnowledgeRepository = agentKnowledgeRepository;
        this.agentToolRepository = agentToolRepository;
        this.agentConversationRepository = agentConversationRepository;
        this.agentChatRepository = agentChatRepository;
        this.agentMessageRepository = agentMessageRepository;
        this.knowledgeRepository = knowledgeRepository;
        this.toolRepository = toolRepository;

        this.kernel = kernel;
        this.proceessSerializer = proceessSerializer;
    }


    protected override async Task<IQueryable<Agent>> CreateFilteredQueryAsync(AgentGetListInputDto? input)
    {
        var query = await base.CreateFilteredQueryAsync(input);
        if (input == null) return query;

        return query.WhereIf(string.IsNullOrEmpty(input?.Keyword) == false, x => x.Name.Contains(input.Keyword))
          .WhereIf(input.Type != null, x => x.Type == input.Type);
    }
    protected override async Task<AgentDto> MapToGetOutputDtoAsync(Agent entity)
    {
        var dto = await base.MapToGetOutputDtoAsync(entity);
        dto.KnowledgeOption = ObjectMapper.Map<AgentKnowledgeOption, AgentKnowledgeOptionDto>(entity.KnowledgeOption);

        var knowledgeIds = entity.Knowledges.Select(x => x.KnowledgeId).ToList();
        var toolIds = entity.Tools.Select(x => x.ToolId).ToList();
        if (knowledgeIds.Count > 0)
        {
            var knowledges = await knowledgeRepository.GetListAsync(x => knowledgeIds.Contains(x.Id));
            dto.Knowledges.ForEach(o =>
            {
                var knowledge = knowledges.FirstOrDefault(x => x.Id == o.KnowledgeId);
                if (knowledge != null)
                {
                    o.Name = knowledge.Name;
                    o.Description = knowledge.Description;
                }
            });
        }
        if (toolIds.Count > 0)
        {
            var tools = await toolRepository.GetListAsync(x => toolIds.Contains(x.Id));
            dto.Tools.ForEach(o =>
            {
                var tool = tools.FirstOrDefault(x => x.Id == o.ToolId);
                if (tool != null)
                {
                    o.Name = tool.Name;
                    o.Description = tool.Description;
                }
            });
        }

        return dto;
    }

    protected override Task UpdateInputMapToEntityAsync(AgentDto updateInput, Agent entity)
    {
        if (updateInput.Questions.Count > 0)
        {
            var questions = MapList<AgentPresetQuestionsDto, AgentPresetQuestions>(updateInput.Questions);
            entity.SyncPresetQuestions(questions);
        }
        if (updateInput.Knowledges.Count > 0)
        {
            var knowledges = MapList<AgentKnowledgeDto, AgentKnowledge>(updateInput.Knowledges);
            entity.SyncKnowledges(knowledges);
        }
        if (updateInput.Tools.Count > 0)
        {
            var tools = MapList<AgentToolDto, AgentTool>(updateInput.Tools);
            entity.SyncTools(tools);
        }
        return base.UpdateInputMapToEntityAsync(updateInput, entity);
    }

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

    #region exec
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
    #endregion
}
