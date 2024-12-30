using Nels.Abp.Ddd.Domain.Services;
using Nels.Aigc.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Nels.Aigc.Services;

public class AgentChatDomainService(IRepository<AgentChat, Guid> repository
    , IRepository<AgentMessage, Guid> messageRepository
    , IRepository<AgentStepLog, Guid> stepLogRepository) : DomainService
{
    public virtual async Task InsertAgentChatAsync(AgentChat agentChat)
    {
        await repository.InsertAsync(agentChat);
        if (agentChat.Messages.Count != 0)
        {
            await messageRepository.InsertManyAsync(agentChat.Messages);
        }
        if (agentChat.StepLogs.Count != 0)
        {
            var stepLogs = agentChat.StepLogs.Cast<AgentStepLog>().ToList();
            if (stepLogs != null)
            {
                await stepLogRepository.InsertManyAsync(stepLogs);
            }
        }
    }
}
