using Volo.Abp.Application.Dtos;

namespace Nels.Abp.Ddd.Application.Contracts.Contracts;

public interface IHasChildrenDto<T, Tkey> : IEntityDto<Tkey> where Tkey : struct
{
    Tkey? ParentId { get; set; }

    List<T>? Children { get; set; }
}
