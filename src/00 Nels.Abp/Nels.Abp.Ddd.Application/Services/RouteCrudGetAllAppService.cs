using Microsoft.AspNetCore.Mvc;
using Nels.Abp.Ddd.Application.Contracts;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace Nels.Abp.Ddd.Application.Services;

public abstract class RouteCrudGetAllAppService<TEntity, TEntityDto, TKey>(IRepository<TEntity, TKey> repository)
    : RouteCrudGetAllAppService<TEntity, TEntityDto, TKey, PagedAndSortedResultRequestDto>(repository)
    where TEntity : class, IEntity<TKey>
    where TEntityDto : IEntityDto<TKey>
    where TKey : struct
{
}

public abstract class RouteCrudGetAllAppService<TEntity, TEntityDto, TKey, TGetListInput>(IRepository<TEntity, TKey> repository)
    : RouteCrudGetAllAppService<TEntity, TEntityDto, TKey, TGetListInput, TEntityDto>(repository)
    where TEntity : class, IEntity<TKey>
    where TEntityDto : IEntityDto<TKey>
    where TKey : struct
{
}

public abstract class RouteCrudGetAllAppService<TEntity, TEntityDto, TKey, TGetListInput, TCreateInput>(IRepository<TEntity, TKey> repository)
    : RouteCrudGetAllAppService<TEntity, TEntityDto, TKey, TGetListInput, TCreateInput, TCreateInput>(repository)
    where TEntity : class, IEntity<TKey>
    where TEntityDto : IEntityDto<TKey>
    where TKey : struct
{
}

public abstract class RouteCrudGetAllAppService<TEntity, TEntityDto, TKey, TGetListInput, TCreateInput, TUpdateInput>(IRepository<TEntity, TKey> repository)
    : RouteCrudGetAllAppService<TEntity, TEntityDto, TEntityDto, TKey, TGetListInput, TCreateInput, TUpdateInput>(repository)
    where TEntity : class, IEntity<TKey>
    where TEntityDto : IEntityDto<TKey>
    where TKey : struct
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

public abstract class RouteCrudGetAllAppService<TEntity, TGetOutputDto, TGetListOutputDto, TKey, TGetListInput, TCreateInput, TUpdateInput>(IRepository<TEntity, TKey> repository)
    : RouteAbstractKeyCrudAppService<TEntity, TGetOutputDto, TGetListOutputDto, TKey, TGetListInput, TCreateInput, TUpdateInput>(repository)
    where TEntity : class, IEntity<TKey>
    where TGetOutputDto : IEntityDto<TKey>
    where TGetListOutputDto : IEntityDto<TKey>
    where TKey : struct
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
    [HttpPost]
    [Route(RemoteServiceConsts.ApiRouteGetAllList)]
    public virtual async Task<List<TGetListOutputDto>> GetAllListAsync(TGetListInput? input)
    {
        await CheckGetListPolicyAsync();

        var query = await CreateFilteredQueryAsync(input);

        query = ApplySorting(query, input);

        var entities = await AsyncExecuter.ToListAsync(query);
        var entityDtos = await MapToGetListOutputDtosAsync(entities);

        //if (entityDtos.Any() && entityDtos.FirstOrDefault() is IHasChildrenDto<TGetListOutputDto, TKey>)
        //{
        //    var hasChildrenDtos = entityDtos.Select(x => (IHasChildrenDto<TGetListOutputDto, TKey>)x).ToList();
        //    var rootDtos = hasChildrenDtos.Where(x => x.ParentId == null).ToList();

        //    if (entityDtos.FirstOrDefault() is IHasOrderCode)
        //    {
        //        rootDtos = rootDtos.Select(x => (IHasOrderCode)x).OrderBy(x => x.Sort).Select(x => (IHasChildrenDto<TGetListOutputDto, TKey>)x).ToList();
        //    }

        //    rootDtos.ForEach(dto => { MapChildren(dto, hasChildrenDtos); });

        //    return new PagedResultDto<TGetListOutputDto>(rootDtos.Count(), rootDtos.Select(x => (TGetListOutputDto)x).ToList());
        //}
        return entityDtos;
    }
}
