using Nels.Aigc.Consts;
using Nels.Aigc.Entities;
using Nels.Aigc.Enums;
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

public class AgentDto : AuditedEntityDto<Guid>
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

    public virtual List<AgentPresetQuestionsDto> Questions { get; set; } = [];

    public virtual List<AgentKnowledgeDto> Knowledges { get; set; } = [];

    public virtual List<AgentToolDto> Tools { get; set; } = [];

    public virtual List<AgentConversationDto> Conversations { get; set; } = [];
}

public class AgentPresetQuestionsDto : EntityDto<Guid>
{
    public virtual Guid AgentId { get; set; }

    [Required]
    [StringLength(AgentPresetQuestionsConsts.MaxContentLength)]
    public virtual string Content { get; set; }

    public virtual int Index { get; set; }
}

public class AgentToolDto : EntityDto<Guid>
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
public class AgentKnowledgeDto : EntityDto<Guid>
{
    public virtual Guid AgentId { get; set; }
    public virtual Guid KnowledgeId { get; set; }
    public virtual string Name { get; set; } = default!;
    public virtual string Icon { get; set; } = default!;
    public virtual string Description { get; set; } = default!;
}
