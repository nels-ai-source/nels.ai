using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Nels.SemanticKernel.Core.Enums;
using Nels.SemanticKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Nels.SemanticKernel.Services;

public class AgentActuator(IAgentService agentService, IChatService chatService, IStreamResponse streamResponse,
    Kernel kernel)
{
    public virtual async Task InvokeStreamingAsync(ChatRequest request, CancellationToken cancellationToken = default)
    {
        IAgent agent = await agentService.GetAgentAsync(request.AgentId) ?? throw new Exception("Agent not found");
        IChat chat = await chatService.NewChatAsync(request.ConversationId, agent.SpaceId) ?? throw new Exception("Conversation not found");
        if (agent.Type == AgentType.ChatCompletion)
        {
            await ChatCompletionAgentInvokeAsync(agent, chat, request, cancellationToken);
        }
        else if (agent.Type == AgentType.Workflow)
        {
        }
        else if (agent.Type == AgentType.MultiAgent)
        {
        }
        else
        {
            throw new NotSupportedException($"Agent type {agent.Type} is not supported.");
        }

    }

    public virtual async Task ChatCompletionAgentInvokeAsync(IAgent agent, IChat chat, ChatRequest request, CancellationToken cancellationToken = default)
    {
        ChatCompletionAgent chatCompletionAgent = CreateChatCompletionAgent(agent);
        ChatHistory chatHistory = await GetChatHistory(chat.ConversationId);

        await streamResponse.ChatCreated(chat.Id, chat.ConversationId);

        PerformanceTracker performanceTracker = new();
        string content = string.Empty;
        string userMessage = request.Messages.FirstOrDefault()?.Content ?? string.Empty;

        chat.AddMessage(await chatService.NewMessageId(), AuthorRole.User, userMessage);

        await foreach (StreamingChatMessageContent response in chatCompletionAgent.InvokeStreamingAsync(
            message: request.Messages.FirstOrDefault()?.Content,
            thread: new ChatHistoryAgentThread(chatHistory),
            options: CreateAgentInvokeOptions(chat),
            cancellationToken: cancellationToken))
        {
            performanceTracker.RecordFirstToken(chat);

            if (string.IsNullOrEmpty(response.Content))
            {
                await HandleEmptyResponse(chat, response);
                continue;
            }
            content += response.Content;
            await streamResponse.MessageDelta(response.Content);
        }
        performanceTracker.Stop(chat);

        await streamResponse.MessageCompleted(content);
        await streamResponse.Down();

        //await chatService.SaveChatMessageAsync(chat);
    }

    private ChatCompletionAgent CreateChatCompletionAgent(IAgent agent)
    {
        return new ChatCompletionAgent
        {
            Instructions = agent.Instructions,
            Kernel = kernel,
            HistoryReducer = new ChatHistoryTruncationReducer(3 * 2),
            Arguments = new KernelArguments(new OpenAIPromptExecutionSettings()
            {
                FunctionChoiceBehavior = FunctionChoiceBehavior.Auto(autoInvoke: true)
            }),
        };
    }
    private static AgentInvokeOptions CreateAgentInvokeOptions(IChat chat)
    {
        return new AgentInvokeOptions
        {
            OnIntermediateMessage = (message) =>
            {
                chat.AddMessage(id: Guid.NewGuid(), content: message);
                return Task.CompletedTask;
            }
        };
    }

    private async Task<ChatHistory> GetChatHistory(Guid conversationId)
    {
        IList<IChatMessage> chatMessages = await chatService.GetChatMessagesAsync(conversationId);
        ChatHistory chatHistory = [];
        if (chatMessages == null)
        {
            return chatHistory;
        }
        foreach (var message in chatMessages)
        {
            var messageContent = JsonSerializer.Deserialize<ChatMessageContent>(message.Metadata);
            if (messageContent == null)
            {
                continue;
            }
            chatHistory.Add(messageContent);
        }
        return chatHistory;
    }

    private static async Task HandleEmptyResponse(IChat chat, StreamingChatMessageContent response)
    {
        var functionCall = response.Items.OfType<StreamingFunctionCallUpdateContent>().SingleOrDefault();
        if (functionCall != null)
        {
            await ProcessStreamingFunctionCall(chat, functionCall);
        }

        if (response.Metadata?["Usage"] is OpenAI.Chat.ChatTokenUsage usage)
        {
            UpdateTokenUsage(chat, usage);
        }
    }
    private static async Task ProcessStreamingFunctionCall(IChat chat, StreamingFunctionCallUpdateContent functionCall)
    {
        if (!string.IsNullOrWhiteSpace(functionCall.CallId))
        {

        }
        await Task.CompletedTask;
    }
    private static void UpdateTokenUsage(IChat chat, OpenAI.Chat.ChatTokenUsage usage)
    {
        chat.InputTokenCount += usage.InputTokenCount;
        chat.OutputTokenCount += usage.OutputTokenCount;
        chat.TotalTokenCount += usage.TotalTokenCount;
    }
    private class PerformanceTracker
    {
        private readonly System.Diagnostics.Stopwatch _stopwatch;
        private bool _isFirstTokenReceived;

        public PerformanceTracker()
        {
            _stopwatch = System.Diagnostics.Stopwatch.StartNew();
            _isFirstTokenReceived = false;
        }

        public void RecordFirstToken(IChat chat)
        {
            if (!_isFirstTokenReceived)
            {
                chat.FirstTokenResponseDuration = Math.Round(_stopwatch.Elapsed.TotalMilliseconds / 1000, 3);
                _isFirstTokenReceived = true;
            }
        }

        public void Stop(IChat chat)
        {
            chat.TotalResponseDuration = Math.Round(_stopwatch.Elapsed.TotalMilliseconds / 1000, 3);
            _stopwatch.Stop();
        }

        public double GetTotalDurationInSeconds()
        {
            return Math.Round(_stopwatch.Elapsed.TotalMilliseconds / 1000, 3);
        }
    }
}