using Volo.Abp.Application.Dtos;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace Nels.Abp.Ddd.Application.Services;

public abstract class RouteCrudAppService<TEntity, TEntityDto, TKey>(IRepository<TEntity, TKey> repository)
    : RouteCrudAppService<TEntity, TEntityDto, TKey, PagedAndSortedResultRequestDto>(repository)
    where TEntity : class, IEntity<TKey>
    where TEntityDto : IEntityDto<TKey>
{
}

public abstract class RouteCrudAppService<TEntity, TEntityDto, TKey, TGetListInput>(IRepository<TEntity, TKey> repository)
    : RouteCrudAppService<TEntity, TEntityDto, TKey, TGetListInput, TEntityDto>(repository)
    where TEntity : class, IEntity<TKey>
    where TEntityDto : IEntityDto<TKey>
{
}

public abstract class RouteCrudAppService<TEntity, TEntityDto, TKey, TGetListInput, TCreateInput>(IRepository<TEntity, TKey> repository)
    : RouteCrudAppService<TEntity, TEntityDto, TKey, TGetListInput, TCreateInput, TCreateInput>(repository)
    where TEntity : class, IEntity<TKey>
    where TEntityDto : IEntityDto<TKey>
{
}

public abstract class RouteCrudAppService<TEntity, TEntityDto, TKey, TGetListInput, TCreateInput, TUpdateInput>(IRepository<TEntity, TKey> repository)
    : RouteCrudAppService<TEntity, TEntityDto, TEntityDto, TKey, TGetListInput, TCreateInput, TUpdateInput>(repository)
    where TEntity : class, IEntity<TKey>
    where TEntityDto : IEntityDto<TKey>
{
    protected override Task<TEntityDto> MapToGetListOutputDtoAsync(TEntity entity)
    {
        return MapToGetOutputDtoAsync(entity);
    }

    protected override TEntityDto MapToGetListOutputDto(TEntity entity)
    {
        return MapToGetOutputDto(entity);
    }
}

public abstract class RouteCrudAppService<TEntity, TGetOutputDto, TGetListOutputDto, TKey, TGetListInput, TCreateInput, TUpdateInput>(IRepository<TEntity, TKey> repository)
    : RouteAbstractKeyCrudAppService<TEntity, TGetOutputDto, TGetListOutputDto, TKey, TGetListInput, TCreateInput, TUpdateInput>(repository)
    where TEntity : class, IEntity<TKey>
    where TGetOutputDto : IEntityDto<TKey>
    where TGetListOutputDto : IEntityDto<TKey>
{
    protected new IRepository<TEntity, TKey> Repository { get; } = repository;

    protected override async Task DeleteByIdAsync(TKey id)
    {
        await Repository.DeleteAsync(id);
    }
    protected override async Task DeleteByIdsAsync(List<TKey> ids)
    {
        await Repository.DeleteManyAsync(ids);
    }
    protected override async Task<TEntity> GetEntityByIdAsync(TKey id)
    {
        return await Repository.GetAsync(id);
    }

    protected override void MapToEntity(TUpdateInput updateInput, TEntity entity)
    {
        if (updateInput is IEntityDto<TKey> entityDto)
        {
            entityDto.Id = entity.Id;
        }

        base.MapToEntity(updateInput, entity);
    }

    protected override IQueryable<TEntity> ApplyDefaultSorting(IQueryable<TEntity> query)
    {
        if (typeof(TEntity).IsAssignableTo<IHasCreationTime>())
        {
            return query.OrderByDescending(e => ((IHasCreationTime)e).CreationTime);
        }
        else
        {
            return query.OrderByDescending(e => e.Id);
        }
    }
}
