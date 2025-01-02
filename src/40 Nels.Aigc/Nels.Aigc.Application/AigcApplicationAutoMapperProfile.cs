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

        CreateMap<Agent, AgentDto>()
            .ForMember(dest => dest.Steps, opt => opt.MapFrom(x => x.Metadata == null ? string.Empty : x.Metadata.Steps))
            .ForMember(dest => dest.States, opt => opt.MapFrom(x => x.Metadata == null ? string.Empty : x.Metadata.States))
            .ReverseMap().Ignore(dest => dest.Metadata);

        CreateMap<AgentPresetQuestions, AgentPresetQuestionsDto>()
            .ReverseMap();

        CreateMap<AgentConversation, AgentConversationDto>().ReverseMap();
        CreateMap<AgentChat, AgentChatDto>().ReverseMap();
        CreateMap<AgentMessage, AgentMessageDto>().ReverseMap();

        CreateMap<Model, ModelDto>().ReverseMap();
        CreateMap<ModelInstance, ModelInstanceDto>().ReverseMap();

        CreateMap<ModelInstanceCreateInputDto, ModelInstance>();
        CreateMap<ModelInstanceUpdateInputDto, ModelInstance>();


        CreateMap<Model, ModelInstance>();

        CreateMap<Space, SpaceDto>().ReverseMap();
        CreateMap<SpaceUser, SpaceUserDto>().ReverseMap();

        CreateMap<Knowledge, KnowledgeDto>().ReverseMap();
        CreateMap<KnowledgeDocument, KnowledgeDocumentDto>().ReverseMap();
        CreateMap<KnowledgeDocumentParagraph, KnowledgeDocumentParagraphDto>().ReverseMap();

        #endregion
    }
}
