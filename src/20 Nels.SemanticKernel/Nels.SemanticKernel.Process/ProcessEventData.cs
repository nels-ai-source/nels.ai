namespace Nels.SemanticKernel.Process;

public class ProcessEventData(Guid messageId, object content, string? name = null)
{
    public Guid MessageId { get; set; } = messageId;
    public object Content { get; set; } = content;

    public string? Name { get; set; } = name;

    public Dictionary<string, object> Properties
    {
        get
        {
            var dic = new Dictionary<string, object>
            {
                { "messageId",MessageId},
                { "content",Content}
            };
            if (Name != null)
            {
                dic.Add("name", Name);
            }
            return dic;
        }
    }
}
