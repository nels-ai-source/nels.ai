namespace Nels.Abp.Ddd.Application.Contracts.Contracts;
public class CheckedOption<TKey> : Option<TKey> where TKey : struct
{
    public CheckedOption() { }

    public virtual bool IsChecked { get; set; }

}
