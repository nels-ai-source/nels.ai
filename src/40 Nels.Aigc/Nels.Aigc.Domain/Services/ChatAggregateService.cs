using Nels.Abp.Ddd.Domain.Services;
using Nels.Aigc.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Nels.Aigc.Services;

public class ChatAggregateService(IRepository<Chat, Guid> repository, IRepository<ChatMessage, Guid> messageRepository) : DomainService
{
    public virtual async Task InsertChatAsync(Chat agentChat, CancellationToken cancellation = default)
    {
        await repository.InsertAsync(agentChat, cancellationToken: cancellation);
        if (agentChat.Messages.Count != 0)
        {
            await messageRepository.InsertManyAsync(agentChat.Messages, cancellationToken: cancellation);
        }
    }
}
