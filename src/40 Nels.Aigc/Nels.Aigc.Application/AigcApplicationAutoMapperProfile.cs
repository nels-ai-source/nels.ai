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

        CreateMap<Agent, AgentDto>();
        CreateMap<AgentDto, Agent>()
            .Ignore(dest => dest.Questions)
            .Ignore(dest => dest.Knowledges)
            .Ignore(dest => dest.Tools);

        CreateMap<AgentPresetQuestions, AgentPresetQuestionsDto>().ReverseMap();
        CreateMap<AgentKnowledge, AgentKnowledgeDto>().ReverseMap();
        CreateMap<AgentTool, AgentToolDto>().ReverseMap();
        CreateMap<AgentKnowledgeOption, AgentKnowledgeOptionDto>().ReverseMap();

        CreateMap<Conversation, ConversationDto>().ReverseMap();
        CreateMap<Chat, ChatDto>().ReverseMap();
        CreateMap<ChatMessage, ChatMessageDto>().ReverseMap();

        CreateMap<Model, ModelDto>().ReverseMap();
        CreateMap<Model, ModelGetListOutputDto>();
        CreateMap<ModelUpdateInputDto, Model>();

        CreateMap<Space, SpaceDto>().ReverseMap();
        CreateMap<SpaceUser, SpaceUserDto>().ReverseMap();

        CreateMap<Knowledge, KnowledgeDto>().ReverseMap();
        CreateMap<KnowledgeDocument, KnowledgeDocumentDto>().ReverseMap();
        CreateMap<KnowledgeDocumentParagraph, KnowledgeDocumentParagraphDto>().ReverseMap();

        #endregion
    }
}
