using Nels.Aigc.Dtos;
using Nels.Aigc.Entities;
using Nels.SemanticKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Nels.Aigc.Services;

public class ChatAppService(IRepository<Conversation, Guid> conversationRepository, IRepository<Chat, Guid> repository, IRepository<ChatMessage, Guid> messageRepository) : AigcAppService, IChatService
{
    public virtual async Task<IList<IChatMessage>> GetChatMessagesAsync(Guid conversationId)
    {
        var entities = await messageRepository.GetListAsync(x => x.ConversationId == conversationId);
        var dtos = ObjectMapper.Map<List<ChatMessage>, List<ChatMessageDto>>(entities);
        return [.. dtos.OrderBy(x => x.Index)];
    }

    public virtual async Task<IChat> NewChatAsync(Guid? conversationId, Guid spaceId)
    {
        Conversation conversation = await conversationRepository.FirstOrDefaultAsync(x => x.Id == conversationId) ??
            new Conversation(GuidGenerator.Create(), spaceId);

        ChatDto dto = new()
        {
            Id = GuidGenerator.Create(),
            ConversationId = conversationId ?? GuidGenerator.Create(),
            SpaceId = spaceId,
            Question = string.Empty,
            Answer = string.Empty,
            Conversation = ObjectMapper.Map<Conversation, ConversationDto>(conversation),
            Messages = []
        };
        return await Task.FromResult(dto);
    }
    public virtual async Task<Guid> NewMessageId()
    {
        return await Task.FromResult(GuidGenerator.Create());
    }
    public virtual async Task SaveChatMessageAsync(IChat chat)
    {
        var conversation = ObjectMapper.Map<ConversationDto, Conversation>((ConversationDto)chat.Conversation) ?? throw new ArgumentNullException(nameof(chat.Conversation), "Conversation cannot be null");

        if (await conversationRepository.AnyAsync(x => x.Id == conversation.Id))
        {
            await conversationRepository.UpdateAsync(conversation);
        }
        else
        {
            await conversationRepository.InsertAsync(conversation);
        }
        var chatEntity = ObjectMapper.Map<ChatDto, Chat>((ChatDto)chat) ?? throw new ArgumentNullException(nameof(chat), "Chat cannot be null");

        await repository.InsertAsync(chatEntity);
    }
}
