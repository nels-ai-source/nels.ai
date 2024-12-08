using System.Collections.Generic;

namespace Nels.Abp.SysMng;
public interface IHasChildren<T, Tkey> where Tkey : struct
{
    Tkey? ParentId { get; set; }

    List<T>? Children { get; set; }
}
