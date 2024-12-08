namespace Nels.Abp.Ddd.Application.Contracts.Contracts;
public class TreeOption<Tkey> : Option<Tkey>, IHasChildrenDto<TreeOption<Tkey>, Tkey> where Tkey : struct
{
    public TreeOption() { }
    public virtual Tkey? ParentId { get; set; }
    public virtual List<TreeOption<Tkey>>? Children { get; set; }
}
