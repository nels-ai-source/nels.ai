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
using Nels.SemanticKernel.Process.Interfaces;
using Nels.SemanticKernel.Process.Steps;
using Nels.SemanticKernel.Process.Variables;
using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Localization;
using Volo.Abp.Uow;

namespace Nels.Aigc.Services;


[Route(AigcRemoteServiceConsts.agentRoute)]
public class AgentAppService : RouteCrudGetAllAppService<Agent, AgentDto, Guid>
{
    private readonly IStreamResponse _streamResponse;
    private readonly Kernel _kernel;
    private readonly IProceessSerializer _proceessSerializer;
    private readonly IRepository<AgentPresetQuestions, Guid> _presetQuestionsRepository;
    private readonly IRepository<AgentMetadata, Guid> _metadataRepository;
    private readonly IRepository<AgentInstance, Guid> _agentInstanceRepository;

    public AgentAppService(IRepository<Agent, Guid> repository,
        IRepository<AgentPresetQuestions, Guid> presetQuestionsRepository,
        IRepository<AgentMetadata, Guid> metadataRepository,
        Kernel kernel,
        ILanguageProvider languageProvider,
        IStreamResponse streamResponse,
        IOptions<AbpLocalizationOptions> localizationOptions,
        IProceessSerializer proceessSerializer) : base(repository)
    {
        CreatePolicyName = AigcPermissions.Agent.Create;
        UpdatePolicyName = AigcPermissions.Agent.Update;
        DeletePolicyName = AigcPermissions.Agent.Delete;
        GetPolicyName = AigcPermissions.Agent.GetList;
        GetListPolicyName = AigcPermissions.Agent.GetList;

        _presetQuestionsRepository = presetQuestionsRepository;
        _metadataRepository = metadataRepository;

        _kernel = kernel;
        _streamResponse = streamResponse;
        _proceessSerializer = proceessSerializer;
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

        entity.PresetQuestions = entity.PresetQuestions.OrderBy(x => x.Index).ToList();
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
    [HttpPost]
    [Route("[action]")]
    public virtual async Task test()
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

        var con = await kernelProcess.StartAsync(_kernel, new KernelProcessEvent { Id = "StartProcess", Data = new StartStepState { UserInput = "123" } });
        var res = await con.GetStateAsync();
        var json = _proceessSerializer.Serialize(res);
        var http = _kernel.GetRequiredService<IHttpContextAccessor>();
        var item = http.HttpContext.Items;
    }

    [HttpPost]
    [Route("[action]")]
    public virtual async Task ExecuteProcess()
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

        var con = await kernelProcess.StartAsync(_kernel, new KernelProcessEvent { Id = "StartProcess", Data = new StartStepState { UserInput = "123" } });

        var res = await con.GetStateAsync();
    }

    [HttpPost]
    [Route("[action]")]
    public virtual async Task AgentStartAsync(AgentStartRequestDto request)
    {
        var agent = await GetAsync(request.AgentId) ?? throw new Exception();
        var instance = request.InstanceId == null
            ? new AgentInstance(GuidGenerator.Create()) : await _agentInstanceRepository.GetAsync(x => x.Id == request.InstanceId.Value);

        if (request.Streaming)
        {
            _streamResponse.EnableStream();
        }

        if (agent.AgentType == AgentType.Llm)
        {
            await LlmAgentStartAsync(request, agent, instance);
        }
        else if (agent.AgentType == AgentType.Workflow)
        {
            await WorkflowAgentStartAsync(request, agent, instance);
        }
    }

    private async Task LlmAgentStartAsync(AgentStartRequestDto request, AgentDto agent, AgentInstance agentInstance)
    {

        KernelProcess kernelProcess = string.IsNullOrWhiteSpace(agentInstance.Steps) ?
            BuilderLlmKernelProcess(request, agent) :
            _proceessSerializer.Deserialize(agentInstance.Steps);

        await kernelProcess.StartAsync(_kernel, new KernelProcessEvent { Id = StepEvent.StartProcessEvent });

    }
    private KernelProcess BuilderLlmKernelProcess(AgentStartRequestDto request, AgentDto agent)
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
            .SendEventTo(new ProcessFunctionTargetBuilder(startStep));

        startStep.OnFunctionResult()
            .SendEventTo(new ProcessFunctionTargetBuilder(llmStep, functionName: StepTypeConst.Llm));

        llmStep.OnFunctionResult()
             .SendEventTo(new ProcessFunctionTargetBuilder(messageStep, functionName: StepTypeConst.Message));

        return process.Build();
    }
    private async Task WorkflowAgentStartAsync(AgentStartRequestDto request, AgentDto agent, AgentInstance agentInstance)
    {
        await Task.CompletedTask;
    }
}
