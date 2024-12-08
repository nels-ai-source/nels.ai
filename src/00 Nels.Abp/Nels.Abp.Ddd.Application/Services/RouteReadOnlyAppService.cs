using Volo.Abp.Application.Dtos;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace Nels.Abp.Ddd.Application.Services;

public abstract class RouteReadOnlyAppService<TEntity, TEntityDto, TKey>(IReadOnlyRepository<TEntity, TKey> repository)
    : RouteReadOnlyAppService<TEntity, TEntityDto, TEntityDto, TKey, PagedAndSortedResultRequestDto>(repository)
    where TEntity : class, IEntity<TKey>
    where TEntityDto : IEntityDto<TKey>
{
}

public abstract class RouteReadOnlyAppService<TEntity, TEntityDto, TKey, TGetListInput>(IReadOnlyRepository<TEntity, TKey> repository)
    : RouteReadOnlyAppService<TEntity, TEntityDto, TEntityDto, TKey, TGetListInput>(repository)
    where TEntity : class, IEntity<TKey>
    where TEntityDto : IEntityDto<TKey>
{
}

public abstract class RouteReadOnlyAppService<TEntity, TGetOutputDto, TGetListOutputDto, TKey, TGetListInput>(IReadOnlyRepository<TEntity, TKey> repository)
    : RouteAbstractKeyReadOnlyAppService<TEntity, TGetOutputDto, TGetListOutputDto, TKey, TGetListInput>(repository)
    where TEntity : class, IEntity<TKey>
    where TGetOutputDto : IEntityDto<TKey>
    where TGetListOutputDto : IEntityDto<TKey>
{
    protected IReadOnlyRepository<TEntity, TKey> Repository { get; } = repository;

    protected override async Task<TEntity> GetEntityByIdAsync(TKey id)
    {
        return await Repository.GetAsync(id);
    }

    protected override IQueryable<TEntity> ApplyDefaultSorting(IQueryable<TEntity> query)
    {
        if (typeof(TEntity).IsAssignableTo<ICreationAuditedObject>())
        {
            return query.OrderByDescending(e => ((ICreationAuditedObject)e).CreationTime);
        }
        else
        {
            return query.OrderByDescending(e => e.Id);
        }
    }

}
