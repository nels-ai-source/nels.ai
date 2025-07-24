using Nels.Aigc.Consts;
using Nels.Aigc.Entities;
using Nels.Aigc.Enums;
using Nels.SemanticKernel.Core.Enums;
using Nels.SemanticKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Volo.Abp.Application.Dtos;

namespace Nels.Aigc.Dtos;

public class AgentDto : FullAuditedEntityDto<Guid>, IAgent, ISpaceIdentifier
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

    public virtual AgentKnowledgeOptionDto KnowledgeOption { get; set; } = new AgentKnowledgeOptionDto();

    public virtual List<ConversationDto> Conversations { get; set; } = [];

    List<IAgentPresetQuestions> IAgent.Questions
    {
        get => [.. Questions.Cast<IAgentPresetQuestions>()];
    }
    List<IAgentKnowledge> IAgent.Knowledges
    {
        get => [.. Knowledges.Cast<IAgentKnowledge>()];
    }
    List<IAgentTool> IAgent.Tools
    {
        get => [.. Tools.Cast<IAgentTool>()];
    }
}

public class AgentUpsertDto : EntityDto<Guid>, ISpaceIdentifier
{
    [Required]
    public virtual Guid SpaceId { get; set; } = Guid.Empty;

    [Required]
    [StringLength(AgentConsts.MaxNameLength)]
    public virtual string Name { get; set; } = default!;

    [StringLength(AgentConsts.MaxIconLength)]
    public virtual string Icon { get; set; } = default!;

    [StringLength(AgentConsts.MaxDescriptionLength)]
    public virtual string Description { get; set; } = default!;

    public virtual AgentType Type { get; set; } = AgentType.ChatCompletion;
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
    public virtual string Name { get; set; } = default!;
    public virtual string? Description { get; set; }
    public virtual List<ToolParamter> InputParamters { get; set; } = [];
    public virtual List<ToolParamter> OutputParamters { get; set; } = [];

}
public class AgentKnowledgeDto : EntityDto<Guid>, IAgentKnowledge
{
    public virtual Guid AgentId { get; set; }
    public virtual Guid KnowledgeId { get; set; }
    public virtual string Name { get; set; }
    public virtual string? Description { get; set; }
}
public class AgentKnowledgeOptionDto : EntityDto<Guid>
{
    public virtual bool AutoInvoke { get; set; } = true;
    public virtual SearchStrategy SearchStrategy { get; set; } = SearchStrategy.Hybrid;
    public virtual int MaxRecallCount { get; set; } = 3;
    public virtual double MinMatchScore { get; set; } = 0.50;
    public virtual ReplyMode ReplyMode { get; set; } = ReplyMode.Default;
    [StringLength(AgentKnowledgeOptionConsts.MaxCustomReplyLength)]
    public virtual string CustomReply { get; set; } = string.Empty;
    public virtual bool ShowSource { get; set; } = true;
    public virtual SourceDisplayMode SourceDisplayMode { get; set; } = SourceDisplayMode.Card;
}

public class AgentGetListInputDto : IPagedResultRequest
{
    public virtual string? Keyword { get; set; }
    public virtual AgentType? Type { get; set; }
    public virtual int SkipCount { get; set; }
    public virtual int MaxResultCount { get; set; }
}
