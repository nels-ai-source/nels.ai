namespace Nels.Abp.Ddd.Application.Contracts.Contracts;

public class TreeCheckedOption<TKey> : CheckedOption<TKey> where TKey : struct
{
    public TreeCheckedOption() { }

    public virtual TKey? ParentValue { get; set; }

    public virtual List<TreeOption<TKey>>? Children { get; set; }
}

