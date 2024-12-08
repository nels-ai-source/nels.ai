namespace Nels.SemanticKernel.Process;

public class ProcessEventData(string stepId, string name, object content)
{
    public string StepId { get; set; } = stepId;
    public string Name { get; set; } = name;
    public object Content { get; set; } = content;

    public Dictionary<string, object> Properties
    {
        get
        {
            return new Dictionary<string, object>
            {
                { nameof(StepId),StepId},
                { nameof(Content),Content}
            };
        }
    }
}
