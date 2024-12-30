namespace Nels.SemanticKernel.Process;

public class StartRequest
{
    public virtual Guid AgentId { get; set; }
    public virtual Guid? AgentConversationId { get; set; }
    public virtual string UserInput { get; set; } = string.Empty;
    public virtual bool Streaming { get; set; }
}
