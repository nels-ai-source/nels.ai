using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nels.SemanticKernel.Consts;

public static class MessageTypeConsts
{
    public const string Question = "question";
    public const string Answer = "answer";
    public const string FunctionCall = "function_call";
    public const string ConversationChatFailed = "conversation_chat_failed";
    public const string ToolOutput = "tool_output";
    public const string ToolResponse = "tool_response";
    public const string FollowUp = "follow_up";
    public const string Verbose = "verbose";
    public const string Knowledge = "knowledge";
}

public static class MessageContentTypeConsts
{
    public const string Text = "text";
    public const string ObjectString = "object_string";
    public const string Card = "card";
}
