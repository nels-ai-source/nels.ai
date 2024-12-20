namespace Nels.SemanticKernel.Process.Logs;

public class ProcessRequestLog
{
    public virtual Guid RequestId { get; set; }
    public virtual string Request { get; set; }
    public virtual string Response { get; set; }
    public virtual double Duration => StepLogs.Sum(x => x.Duration);
    public virtual int PromptTokens => StepLogs.Sum(x => x.PromptTokens);
    public virtual int CompleteTokens => StepLogs.Sum(x => x.CompleteTokens);
    public virtual int Tokens => StepLogs.Sum(x => x.Tokens);
    public virtual List<StepLog> StepLogs { get; set; } = [];
}

public class StepLog
{
    public virtual Guid StepId { get; set; }
    public virtual double Duration { get; set; }
    public virtual int PromptTokens { get; set; } = 0;
    public virtual int CompleteTokens { get; set; } = 0;
    public virtual int Tokens => PromptTokens + CompleteTokens;
}