using Nels.Aigc.Consts;
using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;

namespace Nels.Aigc.Dtos;

public class AgentConversationDto : AuditedEntityDto<Guid>
{
    [Required]
    public virtual Guid AgentId { get; set; }

    [Required]
    [StringLength(AgentConversationConsts.MaxTitleLength)]
    public virtual string Title { get; set; }

    public virtual string Metadata { get; set; } = string.Empty;

}
public class AgentChatDto : AuditedEntityDto<Guid>
{

    [Required]
    public virtual Guid AgentId { get; set; }

    [Required]
    public virtual Guid AgentConversationId { get; set; }

    [StringLength(AgentConversationConsts.MaxQuestionLength)]
    public virtual string Question { get; set; }

    [StringLength(AgentConversationConsts.MaxAnswerLength)]
    public virtual string Answer { get; set; }

    [Required]
    public virtual double Duration { get; set; }

    [Required]
    public virtual int PromptTokens { get; set; }

    [Required]
    public virtual int CompleteTokens { get; set; }
    [Required]
    public virtual int Tokens { get; set; }

}

public class AgentMessageDto : AuditedEntityDto<Guid>
{
    [Required]
    public virtual Guid AgentId { get; set; }

    [Required]
    public virtual Guid AgentConversationId { get; set; }

    [Required]
    public virtual Guid AgentChatId { get; set; }

    [Required]
    [StringLength(AgentMessageConsts.MaxRoleLength)]
    public virtual string Role { get; set; }

    [StringLength(AgentMessageConsts.MaxTypeLength)]
    public virtual string Type { get; set; }

    [StringLength(AgentMessageConsts.MaxContentTypeLength)]
    public virtual string ContentType { get; set; }

    public virtual int Index { get; set; }

    public virtual string Content { get; set; }

    public virtual string Metadata { get; set; }
}

public class AgentStepLogDto : AuditedEntityDto<Guid>
{
    [Required]
    public virtual Guid AgentId { get; set; }

    [Required]
    public virtual Guid AgentConversationId { get; set; }

    [Required]
    public virtual Guid AgentChatId { get; set; }

    [StringLength(AgentStepLogConsts.MaxModelIdLength)]
    public virtual string ModelId { get; set; }

    public virtual int Index { get; set; }
    public virtual Guid StepId { get; set; }
    public virtual double Duration { get; set; } = 0.000;
    public virtual int PromptTokens { get; set; } = 0;
    public virtual int CompleteTokens { get; set; } = 0;
    public virtual int Tokens => PromptTokens + CompleteTokens;
}
