namespace Nels.SemanticKernel.Process
{
    public class StepEvent
    {
        public readonly static string ExecutedEvent = nameof(ExecutedEvent);
        public readonly static string StartProcessEvent = nameof(StartProcessEvent);

        public readonly static string LlmChatMessageEvent = nameof(LlmChatMessageEvent);
    }
}
