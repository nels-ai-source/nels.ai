using Nels.SemanticKernel.Consts;
using System;
using System.Collections.Generic;

namespace Nels.SemanticKernel.Services;

public class ChatRequest
{
    public Guid? ConversationId { get; set; }
    public Guid AgentId { get; set; }
    public List<Message> Messages { get; set; }
}
public class Message
{
    public string Role { get; set; }
    public string Content { get; set; }
    public string ContentType { get; set; } = MessageContentTypeConsts.Text;
    public string Type { get; set; } = MessageTypeConsts.Question;
}
