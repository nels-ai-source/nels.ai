using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Nels.Abp.Ddd.Domain.Services;
using Nels.Aigc.Entities;
using Nels.SemanticKernel.Interfaces;
using Nels.SemanticKernel.Process;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Nels.Aigc.Services;

public class LlmAgentDomainService(
    IStreamResponse streamResponse,
    IRepository<Conversation, Guid> agentConversationRepository,
    IRepository<ChatMessage, Guid> agentMessageRepository,
    ChatAggregateService agentChatDomainService,
    Kernel kernel) : DomainService
{
    public async Task InvokeStreamingAsync(StartRequest request, Entities.Agent agent, CancellationToken cancellation = default)
    {

        AgentGroupChat chat = new();
        LlmAgentRequest llmAgentRequest = await InvokeStreamingProcessAsync(request, agent, chat, cancellation);

        ChatCompletionAgent chatAgent = new()
        {
            Instructions = agent.Instructions,
            Kernel = kernel,
            Arguments = new KernelArguments(new OpenAIPromptExecutionSettings()
            {
                FunctionChoiceBehavior = FunctionChoiceBehavior.Auto(autoInvoke: true)
            }),
            HistoryReducer = new ChatHistoryTruncationReducer(3 * 2),
        };

        var service = kernel.GetRequiredService<IChatCompletionService>();

        await foreach (StreamingChatMessageContent response in chat.InvokeStreamingAsync(chatAgent))
        {
            await streamResponse.MessageDelta(response.Content);
        }

        await foreach (var content in chat.GetChatMessagesAsync(cancellation))
        {
            //if (content.Role != AuthorRole.User)
            //{
            //    llmAgentRequest.Chat.AddMessage(id: llmAgentRequest.MessageId, content: content, insertFirst: true);
            //    continue;
            //}
            //llmAgentRequest.Chat.AddMessage(id: GuidGenerator.Create(), content: content, insertFirst: true);
        }
        llmAgentRequest.Conversation.SetTitle(string.IsNullOrWhiteSpace(llmAgentRequest.Conversation.Title) ? request.UserInput : llmAgentRequest.Conversation.Title);

        llmAgentRequest.Conversation = request.AgentConversationId == null ?
            await agentConversationRepository.InsertAsync(llmAgentRequest.Conversation, cancellationToken: cancellation) :
            await agentConversationRepository.UpdateAsync(llmAgentRequest.Conversation, cancellationToken: cancellation);
        await agentChatDomainService.InsertChatAsync(llmAgentRequest.Chat, cancellation);

    }

    private async Task<LlmAgentRequest> InvokeStreamingProcessAsync(StartRequest request, Entities.Agent agent, AgentGroupChat chat, CancellationToken cancellation = default)
    {
        Guid conversation = request.AgentConversationId ?? GuidGenerator.Create();
        LlmAgentRequest llmAgentRequest = new()
        {
            Agent = agent,
            Conversation = request.AgentConversationId == null ?
               new Conversation(conversation, agent.SpaceId) :
               await agentConversationRepository.FirstOrDefaultAsync(x => x.Id == request.AgentConversationId.Value, cancellationToken: cancellation) ?? new Conversation(conversation, agent.SpaceId),
            Chat = new(GuidGenerator.Create(), agent.SpaceId, conversation),
            MessageId = GuidGenerator.Create(),
        };

        List<ChatMessage> messages = await agentMessageRepository.GetListAsync(x => x.ChatId == agent.Id, cancellationToken: cancellation);
        foreach (var item in messages)
        {
            chat.AddChatMessage(new Microsoft.SemanticKernel.ChatMessageContent(new AuthorRole(item.Role), item.Content));
        }
        Microsoft.SemanticKernel.ChatMessageContent message = new(AuthorRole.User, request.UserInput);
        chat.AddChatMessage(message);

        return llmAgentRequest;
    }
}
public class LlmAgentRequest
{
    public virtual Entities.Agent Agent { get; set; }
    public virtual Conversation Conversation { get; set; }
    public virtual Entities.Chat Chat { get; set; }
    public virtual Guid MessageId { get; set; }
}