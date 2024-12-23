using Nels.SemanticKernel.Process.Consts;

namespace Nels.SemanticKernel.Process.Interfaces;

public interface IAgentChat
{
    Guid Id { get; }
    Guid AgentId { get; set; }
    Guid AgentConversationId { get; set; }
    string Question { get; set; }
    string Answer { get; set; }
    IStepLog AddStepLog(Guid stepId);
    void AddMessage(string role, string content, string type = MessageTypeConsts.Answer, string contentType = MessageContentTypeConsts.Text, string? metadata = null);
}

public interface IStepLog
{
    Guid StepId { get; set; }
    double Duration { get; }
    int PromptTokens { get; set; }
    int CompleteTokens { get; set; }
    int Tokens => PromptTokens + CompleteTokens;
    void SetDuration(Double millisecond);
}