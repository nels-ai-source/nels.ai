namespace Nels.Abp.Ddd.Application.Contracts.Contracts;
public class Option<Tkey> where Tkey : struct
{
    public Option() { }

    public virtual Tkey Id { get; set; }

    public virtual string Value { get; set; } = string.Empty;

    public virtual string Text { get; set; } = string.Empty;
    public virtual string Label => Text;
    public virtual object? Tag { get; set; }
}

