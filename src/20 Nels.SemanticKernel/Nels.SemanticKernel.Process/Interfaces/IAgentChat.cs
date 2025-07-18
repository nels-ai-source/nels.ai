using Microsoft.SemanticKernel;
using Nels.SemanticKernel.Process.Consts;

namespace Nels.SemanticKernel.Process.Interfaces;

public interface IAgentChat
{
    Guid Id { get; }
    Guid AgentId { get; set; }
    Guid ConversationId { get; set; }
    string Question { get; set; }
    string Answer { get; set; }
    IStepLog AddStepLog(Guid id, Guid stepId);
    void AddMessage(Guid id, ChatMessageContent content, string? metadata = null, bool insertFirst = false);
    void AddMessage(Guid id, string role, string content, string type = MessageTypeConsts.Answer, string contentType = MessageContentTypeConsts.Text, string? metadata = null, bool insertFirst = false);
}

public interface IStepLog
{
    Guid StepId { get; set; }
    string ModelId { get; set; }
    double Duration { get; }
    int PromptTokens { get; set; }
    int CompleteTokens { get; set; }
    int Tokens => PromptTokens + CompleteTokens;
    void SetDuration(Double millisecond);
}