using DocumentFormat.OpenXml.Math;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents;
using Microsoft.SemanticKernel.Agents.History;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Nels.Abp.Ddd.Domain.Services;
using Nels.Aigc.Entities;
using Nels.SemanticKernel.Interfaces;
using Nels.SemanticKernel.Process;
using Nels.SemanticKernel.Process.Consts;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Nels.Aigc.Services;

public class LlmAgentDomainService(
    IStreamResponse streamResponse,
    IRepository<AgentConversation, Guid> agentConversationRepository,
    IRepository<AgentMessage, Guid> agentMessageRepository,
    AgentChatDomainService agentChatDomainService,
    Kernel kernel) : DomainService
{
    public async Task InvokeStreamingAsync(StartRequest request, AgentEntity agent, CancellationToken cancellation = default)
    {
        if (agent.Metadata is LlmAgentMetadata metadata)
        {
            AgentGroupChat chat = new();
            LlmAgentRequest llmAgentRequest = await InvokeStreamingProcessAsync(request, agent, chat, cancellation);

            ChatCompletionAgent chatAgent = new()
            {
                Instructions = metadata.Prompt,
                Kernel = kernel,
                Arguments = new KernelArguments(new OpenAIPromptExecutionSettings()
                {
                    FunctionChoiceBehavior = FunctionChoiceBehavior.Auto(autoInvoke: metadata.ToolAutoInvoke)
                }),
                HistoryReducer = metadata.ChatReducerCount > 0 ? new ChatHistoryTruncationReducer(metadata.ChatReducerCount * 2) : null,
            };

            await foreach (StreamingChatMessageContent response in chat.InvokeStreamingAsync(chatAgent))
            {
                await streamResponse.WriteMessagAsync(llmAgentRequest.MessageId, response.Content);
            }

            await foreach (var content in chat.GetChatMessagesAsync(cancellation))
            {
                if (content.Role != AuthorRole.User)
                {
                    llmAgentRequest.Chat.AddMessage(id: llmAgentRequest.MessageId, content: content, insertFirst: true);
                    continue;
                }
                llmAgentRequest.Chat.AddMessage(id: GuidGenerator.Create(), content: content, insertFirst: true);
            }
            llmAgentRequest.Conversation.SetTitle(string.IsNullOrWhiteSpace(llmAgentRequest.Conversation.Title) ? request.UserInput : llmAgentRequest.Conversation.Title);

            llmAgentRequest.Conversation = request.AgentConversationId == null ?
                await agentConversationRepository.InsertAsync(llmAgentRequest.Conversation, cancellationToken: cancellation) :
                await agentConversationRepository.UpdateAsync(llmAgentRequest.Conversation, cancellationToken: cancellation);
            await agentChatDomainService.InsertAgentChatAsync(llmAgentRequest.Chat, cancellation);
        }
    }

    private async Task<LlmAgentRequest> InvokeStreamingProcessAsync(StartRequest request, AgentEntity agent, AgentGroupChat chat, CancellationToken cancellation = default)
    {
        Guid conversation = request.AgentConversationId ?? GuidGenerator.Create();
        LlmAgentRequest llmAgentRequest = new()
        {
            Agent = agent,
            Conversation = request.AgentConversationId == null ?
               new AgentConversation(conversation, agent.Id) :
               await agentConversationRepository.GetAsync(x => x.Id == request.AgentConversationId.Value),
            Chat = new(GuidGenerator.Create(), agent.Id, conversation),
            MessageId = GuidGenerator.Create(),
        };

        List<AgentMessage> messages = await agentMessageRepository.GetListAsync(x => x.AgentChatId == agent.Id, cancellationToken: cancellation);
        foreach (var item in messages)
        {
            chat.AddChatMessage(new ChatMessageContent(new AuthorRole(item.Role), item.Content));
        }
        ChatMessageContent message = new(AuthorRole.User, request.UserInput);
        chat.AddChatMessage(message);

        return llmAgentRequest;
    }
}
public class LlmAgentRequest
{
    public virtual AgentEntity Agent { get; set; }
    public virtual AgentConversation Conversation { get; set; }
    public virtual Entities.AgentChat Chat { get; set; }
    public virtual Guid MessageId { get; set; }
}