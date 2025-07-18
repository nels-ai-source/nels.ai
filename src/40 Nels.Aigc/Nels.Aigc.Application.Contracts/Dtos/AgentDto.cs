using Nels.Aigc.Consts;
using Nels.Aigc.Entities;
using Nels.Aigc.Enums;
using Nels.SemanticKernel.Core.Enums;
using Nels.SemanticKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;

namespace Nels.Aigc.Dtos;

public class LlmAgentDto : AgentDto
{
    public virtual string Prompt { get; set; } = string.Empty;
    public virtual int ChatReducerCount { get; set; } = 0;
    public virtual bool ToolAutoInvoke { get; set; } = false;
}
public class WorkflowAgentDto : AgentDto
{
    public virtual string Steps { get; set; } = string.Empty;
    public virtual string States { get; set; } = string.Empty;
}

public class AgentDto : AuditedEntityDto<Guid>, IAgent
{
    [Required]
    public virtual Guid SpaceId { get; set; } = Guid.Empty;

    [StringLength(AgentConsts.MaxNameLength)]
    public virtual string Name { get; set; } = default!;

    [StringLength(AgentConsts.MaxIconLength)]
    public virtual string Icon { get; set; } = default!;

    [StringLength(AgentConsts.MaxDescriptionLength)]
    public virtual string Description { get; set; } = default!;
    public virtual AgentType Type { get; set; } = AgentType.ChatCompletion;
    public virtual string Instructions { get; set; } = string.Empty;

    [StringLength(AgentConsts.MaxPrologueLength)]
    public virtual string Prologue { get; set; } = string.Empty;

    public virtual List<IAgentPresetQuestions> Questions { get; set; } = [];

    public virtual List<IAgentKnowledge> Knowledges { get; set; } = [];

    public virtual List<IAgentTool> Tools { get; set; } = [];

    public virtual List<ConversationDto> Conversations { get; set; } = [];
}

public class AgentPresetQuestionsDto : EntityDto<Guid>, IAgentPresetQuestions
{
    public virtual Guid AgentId { get; set; }

    [Required]
    [StringLength(AgentPresetQuestionsConsts.MaxContentLength)]
    public virtual string Content { get; set; }

    public virtual int Index { get; set; }
}

public class AgentToolDto : EntityDto<Guid>, IAgentTool
{
    public virtual Guid AgentId { get; set; } = default!;
    public virtual Guid ToolId { get; set; } = default!;
    public virtual string PluginName { get; set; } = default!;
    public virtual string Icon { get; set; } = default!;
    public virtual string Name { get; set; } = default!;
    public virtual string Description { get; set; } = default!;

    public virtual List<ToolParamter> InputParamters { get; set; } = [];
    public virtual List<ToolParamter> OutputParamters { get; set; } = [];

}
public class AgentKnowledgeDto : EntityDto<Guid>, IAgentKnowledge
{
    public virtual Guid AgentId { get; set; }
    public virtual Guid KnowledgeId { get; set; }
    public virtual string Name { get; set; } = default!;
    public virtual string Icon { get; set; } = default!;
    public virtual string Description { get; set; } = default!;
}
