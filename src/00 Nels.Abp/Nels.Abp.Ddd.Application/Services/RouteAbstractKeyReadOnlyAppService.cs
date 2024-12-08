using Microsoft.AspNetCore.Mvc;
using Nels.Abp.Ddd.Application.Contracts;
using System.Linq.Dynamic.Core;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Localization;
using Volo.Abp.ObjectMapping;

namespace Nels.Abp.Ddd.Application.Services;
public abstract class RouteAbstractKeyReadOnlyAppService<TEntity, TEntityDto, TKey>(IReadOnlyRepository<TEntity> repository)
    : RouteAbstractKeyReadOnlyAppService<TEntity, TEntityDto, TEntityDto, TKey, PagedAndSortedResultRequestDto>(repository)
    where TEntity : class, IEntity
{
}

public abstract class RouteAbstractKeyReadOnlyAppService<TEntity, TEntityDto, TKey, TGetListInput>(IReadOnlyRepository<TEntity> repository)
    : RouteAbstractKeyReadOnlyAppService<TEntity, TEntityDto, TEntityDto, TKey, TGetListInput>(repository)
    where TEntity : class, IEntity
{
}

public abstract class RouteAbstractKeyReadOnlyAppService<TEntity, TGetOutputDto, TGetListOutputDto, TKey, TGetListInput>(IReadOnlyRepository<TEntity> repository)
    : ApplicationService
    , IReadOnlyAppService<TGetOutputDto, TGetListOutputDto, TKey, TGetListInput>
    where TEntity : class, IEntity
{
    protected IReadOnlyRepository<TEntity> ReadOnlyRepository { get; } = repository;

    protected virtual string GetPolicyName { get; set; } = string.Empty;

    protected virtual string GetListPolicyName { get; set; } = string.Empty;

    [HttpPost]
    [Route(RemoteServiceConsts.ApiRouteGet)]
    public virtual async Task<TGetOutputDto> GetAsync(TKey id)
    {
        await CheckGetPolicyAsync();

        var entity = await GetEntityByIdAsync(id);

        return await MapToGetOutputDtoAsync(entity);
    }
    [HttpPost]
    [Route(RemoteServiceConsts.ApiRouteGetList)]
    public virtual async Task<PagedResultDto<TGetListOutputDto>> GetListAsync(TGetListInput input)
    {
        await CheckGetListPolicyAsync();

        var query = await CreateFilteredQueryAsync(input);
        var totalCount = await AsyncExecuter.CountAsync(query);

        var entityDtos = new List<TGetListOutputDto>();

        if (totalCount > 0)
        {
            query = ApplySorting(query, input);
            query = ApplyPaging(query, input);

            entityDtos = await MapToGetListOutputDtosAsync(await AsyncExecuter.ToListAsync(query));
        }

        return new PagedResultDto<TGetListOutputDto>(
            totalCount,
            entityDtos
        );
    }
    protected abstract Task<TEntity> GetEntityByIdAsync(TKey id);

    protected virtual async Task CheckGetPolicyAsync()
    {
        await CheckPolicyAsync(GetPolicyName);
    }

    protected virtual async Task CheckGetListPolicyAsync()
    {
        await CheckPolicyAsync(GetListPolicyName);
    }

    /// <summary>
    /// Should apply sorting if needed.
    /// </summary>
    /// <param name="query">The query.</param>
    /// <param name="input">The input.</param>
    protected virtual IQueryable<TEntity> ApplySorting(IQueryable<TEntity> query, TGetListInput? input)
    {
        //Try to sort query if available
        if (input != null && input is ISortedResultRequest sortInput)
        {
            if (!sortInput.Sorting.IsNullOrWhiteSpace())
            {
                return query.OrderBy(sortInput.Sorting);
            }
        }

        //IQueryable.Task requires sorting, so we should sort if Take will be used.
        if (input != null && input is ILimitedResultRequest)
        {
            return ApplyDefaultSorting(query);
        }

        //No sorting
        return query;
    }

    /// <summary>
    /// Applies sorting if no sorting specified but a limited result requested.
    /// </summary>
    /// <param name="query">The query.</param>
    protected virtual IQueryable<TEntity> ApplyDefaultSorting(IQueryable<TEntity> query)
    {
        if (typeof(TEntity).IsAssignableTo<IHasCreationTime>())
        {
            return query.OrderByDescending(e => ((IHasCreationTime)e).CreationTime);
        }

        throw new AbpException("No sorting specified but this query requires sorting. Override the ApplyDefaultSorting method for your application service derived from AbstractKeyReadOnlyAppService!");
    }

    /// <summary>
    /// Should apply paging if needed.
    /// </summary>
    /// <param name="query">The query.</param>
    /// <param name="input">The input.</param>
    protected virtual IQueryable<TEntity> ApplyPaging(IQueryable<TEntity> query, TGetListInput input)
    {
        //Try to use paging if available
        if (input is IPagedResultRequest pagedInput)
        {
            return query.PageBy(pagedInput);
        }

        //Try to limit query result if available
        if (input is ILimitedResultRequest limitedInput)
        {
            return query.Take(limitedInput.MaxResultCount);
        }

        //No paging
        return query;
    }

    /// <summary>
    /// This method should create <see cref="IQueryable{TEntity}"/> based on given input.
    /// It should filter query if needed, but should not do sorting or paging.
    /// Sorting should be done in <see cref="ApplySorting"/> and paging should be done in <see cref="ApplyPaging"/>
    /// methods.
    /// </summary>
    /// <param name="input">The input.</param>
    protected virtual async Task<IQueryable<TEntity>> CreateFilteredQueryAsync(TGetListInput? input)
    {
        return await ReadOnlyRepository.GetQueryableAsync();
    }

    /// <summary>
    /// Maps <typeparamref name="TEntity"/> to <typeparamref name="TGetOutputDto"/>.
    /// It internally calls the <see cref="MapToGetOutputDto"/> by default.
    /// It can be overriden for custom mapping.
    /// Overriding this has higher priority than overriding the <see cref="MapToGetOutputDto"/>
    /// </summary>
    protected virtual Task<TGetOutputDto> MapToGetOutputDtoAsync(TEntity entity)
    {
        return Task.FromResult(MapToGetOutputDto(entity));
    }

    /// <summary>
    /// Maps <typeparamref name="TEntity"/> to <typeparamref name="TGetOutputDto"/>.
    /// It uses <see cref="IObjectMapper"/> by default.
    /// It can be overriden for custom mapping.
    /// </summary>
    protected virtual TGetOutputDto MapToGetOutputDto(TEntity entity)
    {
        return ObjectMapper.Map<TEntity, TGetOutputDto>(entity);
    }

    /// <summary>
    /// Maps a list of <typeparamref name="TEntity"/> to <typeparamref name="TGetListOutputDto"/> objects.
    /// It uses <see cref="MapToGetListOutputDtoAsync"/> method for each item in the list.
    /// </summary>
    protected virtual async Task<List<TGetListOutputDto>> MapToGetListOutputDtosAsync(List<TEntity> entities)
    {
        var dtos = new List<TGetListOutputDto>();

        foreach (var entity in entities)
        {
            dtos.Add(await MapToGetListOutputDtoAsync(entity));
        }

        return dtos;
    }

    /// <summary>
    /// Maps <typeparamref name="TEntity"/> to <typeparamref name="TGetListOutputDto"/>.
    /// It internally calls the <see cref="MapToGetListOutputDto"/> by default.
    /// It can be overriden for custom mapping.
    /// Overriding this has higher priority than overriding the <see cref="MapToGetListOutputDto"/>
    /// </summary>
    protected virtual Task<TGetListOutputDto> MapToGetListOutputDtoAsync(TEntity entity)
    {
        return Task.FromResult(MapToGetListOutputDto(entity));
    }

    /// <summary>
    /// Maps <typeparamref name="TEntity"/> to <typeparamref name="TGetListOutputDto"/>.
    /// It uses <see cref="IObjectMapper"/> by default.
    /// It can be overriden for custom mapping.
    /// </summary>
    protected virtual TGetListOutputDto MapToGetListOutputDto(TEntity entity)
    {
        return ObjectMapper.Map<TEntity, TGetListOutputDto>(entity);
    }
    public virtual TDestination Map<TSource, TDestination>(TSource source)
    {
        return ObjectMapper.Map<TSource, TDestination>(source);
    }
    public virtual List<TDestination> MapList<TSource, TDestination>(List<TSource> source)
    {
        return ObjectMapper.Map<List<TSource>, List<TDestination>>(source);
    }

    protected string GetDisplayName(string name)
    {
        name = name.Split(',').Last();
        var localizable = L[name];
        if (localizable == null) return name;

        return localizable.Value ?? string.Empty;
    }
}
