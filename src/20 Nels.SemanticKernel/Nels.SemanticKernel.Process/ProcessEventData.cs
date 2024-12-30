namespace Nels.SemanticKernel.Process;

public class ProcessEventData(Guid messageId, string name, object content)
{
    public Guid MessageId { get; set; } = messageId;
    public string Name { get; set; } = name;
    public object Content { get; set; } = content;

    public Dictionary<string, object> Properties
    {
        get
        {
            return new Dictionary<string, object>
            {
                { "messageId",MessageId},
                { "content",Content}
            };
        }
    }
}
