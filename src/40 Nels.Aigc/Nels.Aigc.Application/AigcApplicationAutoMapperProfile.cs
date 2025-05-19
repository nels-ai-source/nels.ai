using AutoMapper;
using Nels.Aigc.Dtos;
using Nels.Aigc.Entities;
using System;
using Volo.Abp.AutoMapper;

namespace Nels.Aigc;

public class AigcApplicationAutoMapperProfile : Profile
{
    public AigcApplicationAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */

        #region aigc
        CreateMap<Prompt, PromptDto>().ReverseMap();

        CreateMap<AgentEntity, AgentDto>()
            .ReverseMap().Ignore(dest => dest.Metadata);

        CreateMap<AgentEntity, LlmAgentDto>()
            .Ignore(dest => dest.Prompt)
            .ReverseMap().Ignore(dest => dest.Metadata);

        CreateMap<LlmAgentMetadata, LlmAgentDto>()
            .ForMember(dest => dest.Prompt, opt => opt.MapFrom(src => src.Prompt))
            .ForMember(dest => dest.ChatReducerCount, opt => opt.MapFrom(src => src.ChatReducerCount))
            .ForMember(dest => dest.ToolAutoInvoke, opt => opt.MapFrom(src => src.ToolAutoInvoke))
            .Ignore(dest => dest.Id)
            .Ignore(dest => dest.CreatorId)
            .Ignore(dest => dest.CreationTime)
            .Ignore(dest => dest.LastModifierId)
            .Ignore(dest => dest.LastModificationTime)
            .ReverseMap()
            .Ignore(dest => dest.Id)
            .Ignore(dest => dest.CreatorId)
            .Ignore(dest => dest.CreationTime)
            .Ignore(dest => dest.LastModifierId);


        CreateMap<AgentEntity, WorkflowAgentDto>()
            .Ignore(dest => dest.States)
            .Ignore(dest => dest.Steps)
            .ReverseMap().Ignore(dest => dest.Metadata);

        CreateMap<WorkflowAgentMetadata, WorkflowAgentDto>()
            .ReverseMap();

        CreateMap<AgentPresetQuestions, AgentPresetQuestionsDto>()
            .ReverseMap();

        CreateMap<AgentConversationEntity, AgentConversationDto>().ReverseMap();
        CreateMap<AgentChat, AgentChatDto>().ReverseMap();
        CreateMap<AgentMessage, AgentMessageDto>().ReverseMap();

        CreateMap<Model, ModelDto>().ReverseMap();
        CreateMap<Model, ModelGetListOutputDto>();

        CreateMap<Space, SpaceDto>().ReverseMap();
        CreateMap<SpaceUser, SpaceUserDto>().ReverseMap();

        CreateMap<Knowledge, KnowledgeDto>().ReverseMap();
        CreateMap<KnowledgeDocument, KnowledgeDocumentDto>().ReverseMap();
        CreateMap<KnowledgeDocumentParagraph, KnowledgeDocumentParagraphDto>().ReverseMap();

        #endregion
    }
}
