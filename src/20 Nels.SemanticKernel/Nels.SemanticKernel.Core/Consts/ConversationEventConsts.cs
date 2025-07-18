namespace Nels.SemanticKernel.Consts;

public static class ConversationEventConsts
{
    private const string ConversationPrefix = "conversation";

    public const string ChatCreated = ConversationPrefix + ".chat.created";
    public const string ChatInProgress = ConversationPrefix + ".chat.in_progress";
    public const string ChatFailed = ConversationPrefix + ".chat.failed";

    public const string MessageCompleted = ConversationPrefix + ".message.completed";
    public const string MessageDelta = ConversationPrefix + ".message.delta";

    public const string Done = "done";
}
