using Microsoft.SemanticKernel;
using System;
using System.Collections.Generic;

namespace Nels.SemanticKernel.Services;

public class AgentRequest
{
    public Guid BotId { get; set; }
    public Guid? ConversationId { get; set; }
    public bool Stream { get; set; } = true;
    public List<ChatMessageContent> Messages { get; set; } = [];
}
