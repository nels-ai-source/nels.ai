using Nels.Aigc.Consts;
using Nels.Aigc.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;

namespace Nels.Aigc.Dtos;

public class AgentDto : AuditedEntityDto<Guid>
{
    [StringLength(AgentConsts.MaxNameLength)]
    public virtual string Name { get; set; } = default!;

    [StringLength(AgentConsts.MaxDescriptionLength)]
    public virtual string Description { get; set; } = default!;
    public virtual AgentType AgentType { get; set; } = AgentType.Llm;
    public virtual string IntroductionText { get; set; } = string.Empty;
    public virtual List<AgentPresetQuestionsDto> PresetQuestions { get; set; } = [];
    public virtual string? Metadata { get; set; } = string.Empty;
}

public class AgentPresetQuestionsDto
{
    public virtual Guid Id { get; set; }
    public virtual string Content { get; set; }
    public virtual int Index { get; set; }
}
