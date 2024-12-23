using Nels.SemanticKernel.Process.Interfaces;

namespace Nels.SemanticKernel.Process;

public class ProcessState
{
    public virtual Guid AgentId { get; set; }
    public virtual Guid AgentConversationId { get; set; }
    public virtual string UserInput { get; set; } = string.Empty;
    public virtual bool Streaming { get; set; }
    public Dictionary<string, object> Context { get; set; } = [];
    public virtual IAgentChat AgentChat { get; set; }
}
