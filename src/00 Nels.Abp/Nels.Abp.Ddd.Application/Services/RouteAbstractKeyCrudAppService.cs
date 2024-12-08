using Microsoft.AspNetCore.Mvc;
using Nels.Abp.Ddd.Application.Contracts;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.MultiTenancy;
using Volo.Abp.ObjectMapping;

namespace Nels.Abp.Ddd.Application.Services;

public abstract class RouteAbstractKeyCrudAppService<TEntity, TEntityDto, TKey>(IRepository<TEntity> repository)
    : RouteAbstractKeyCrudAppService<TEntity, TEntityDto, TKey, PagedAndSortedResultRequestDto>(repository)
    where TEntity : class, IEntity
{
}

public abstract class RouteAbstractKeyCrudAppService<TEntity, TEntityDto, TKey, TGetListInput>(IRepository<TEntity> repository)
    : RouteAbstractKeyCrudAppService<TEntity, TEntityDto, TKey, TGetListInput, TEntityDto, TEntityDto>(repository)
    where TEntity : class, IEntity
{
}

public abstract class RouteAbstractKeyCrudAppService<TEntity, TEntityDto, TKey, TGetListInput, TCreateInput>(IRepository<TEntity> repository)
    : RouteAbstractKeyCrudAppService<TEntity, TEntityDto, TKey, TGetListInput, TCreateInput, TCreateInput>(repository)
    where TEntity : class, IEntity
{
}

public abstract class RouteAbstractKeyCrudAppService<TEntity, TEntityDto, TKey, TGetListInput, TCreateInput, TUpdateInput>(IRepository<TEntity> repository)
    : RouteAbstractKeyCrudAppService<TEntity, TEntityDto, TEntityDto, TKey, TGetListInput, TCreateInput, TUpdateInput>(repository)
    where TEntity : class, IEntity
{
    /// <summary>
    /// Asynchronously maps the entity to a list output DTO.
    /// </summary>
    /// <param name="entity">The entity instance.</param>
    /// <returns>A task that represents the asynchronous operation and returns the mapped entity DTO.</returns>
    protected override Task<TEntityDto> MapToGetListOutputDtoAsync(TEntity entity)
    {
        return MapToGetOutputDtoAsync(entity);
    }

    /// <summary>
    /// Maps the entity to a list output DTO.
    /// </summary>
    /// <param name="entity">The entity instance.</param>
    /// <returns>The mapped entity DTO.</returns>
    protected override TEntityDto MapToGetListOutputDto(TEntity entity)
    {
        return MapToGetOutputDto(entity);
    }
}

public abstract class RouteAbstractKeyCrudAppService<TEntity, TGetOutputDto, TGetListOutputDto, TKey, TGetListInput, TCreateInput, TUpdateInput>(IRepository<TEntity> repository)
    : RouteAbstractKeyReadOnlyAppService<TEntity, TGetOutputDto, TGetListOutputDto, TKey, TGetListInput>(repository),
        ICrudAppService<TGetOutputDto, TGetListOutputDto, TKey, TGetListInput, TCreateInput, TUpdateInput>
    where TEntity : class, IEntity
{
    protected IRepository<TEntity> Repository { get; } = repository;

    protected virtual string CreatePolicyName { get; set; } = string.Empty;

    protected virtual string UpdatePolicyName { get; set; } = string.Empty;

    protected virtual string DeletePolicyName { get; set; } = string.Empty;

    /// <summary>
    /// Asynchronously creates an entity and returns the information of the newly created entity.
    /// </summary>
    /// <param name="input">The input data required to create the entity.</param>
    /// <returns>A task that will return the output data of the newly created entity.</returns>
    [HttpPost]
    [Route(RemoteServiceConsts.ApiRouteCreate)]
    public virtual async Task<TGetOutputDto> CreateAsync(TCreateInput input)
    {
        await CheckCreatePolicyAsync();

        var entity = await CreateMapToEntityAsync(input);

        TryToSetTenantId(entity);

        await ProcessCreate(entity);

        await ValidateCreate(entity);

        await InsertAsync(entity);

        return await MapToGetOutputDtoAsync(entity);
    }
    /// <summary>
    /// Asynchronously updates an existing entity and returns the updated entity's information.
    /// </summary>
    /// <param name="id">The unique identifier of the entity to update.</param>
    /// <param name="input">The input data required to update the entity.</param>
    /// <returns>A task that will return the output data of the updated entity.</returns>
    [HttpPost]
    [Route(RemoteServiceConsts.ApiRouteUpdate)]
    public virtual async Task<TGetOutputDto> UpdateAsync(TKey id, TUpdateInput input)
    {
        await CheckUpdatePolicyAsync();

        var entity = await GetEntityByIdAsync(id);

        await UpdateInputMapToEntityAsync(input, entity);

        await ProcessUpdate(input, entity);

        await ValidateUpdate(input, entity);

        await UpdateAsync(entity);

        return await MapToGetOutputDtoAsync(entity);
    }
    /// <summary>
    /// Asynchronously deletes an entity by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the entity to delete.</param>
    /// <returns>A task representing the asynchronous delete operation.</returns>
    [HttpPost]
    [Route(RemoteServiceConsts.ApiRouteDelete)]
    public virtual async Task DeleteAsync(TKey id)
    {
        await CheckDeletePolicyAsync();

        await ValidateDelete(id);

        await DeleteByIdAsync(id);
    }
    /// <summary>
    /// Asynchronously deletes multiple entities by their unique identifiers.
    /// </summary>
    /// <param name="ids">A list of unique identifiers of the entities to delete.</param>
    /// <returns>A task representing the asynchronous delete operation.</returns>
    [HttpPost]
    [Route(RemoteServiceConsts.ApiRouteDeleteMany)]
    public virtual async Task DeleteManyAsync(List<TKey> ids)
    {
        await CheckDeletePolicyAsync();

        await DeleteByIdsAsync(ids);
    }

    protected virtual Task ProcessCreate(TEntity entity) { return Task.CompletedTask; }
    protected virtual Task ProcessUpdate(TUpdateInput input, TEntity entity) { return Task.CompletedTask; }
    protected virtual Task ValidateCreate(TEntity entity) { return Task.CompletedTask; }
    protected virtual Task ValidateUpdate(TUpdateInput input, TEntity entity) { return Task.CompletedTask; }
    protected virtual Task ValidateDelete(TKey id) { return Task.CompletedTask; }

    protected abstract Task DeleteByIdAsync(TKey id);
    protected abstract Task DeleteByIdsAsync(List<TKey> ids);
    /// <summary>
    /// Checks if the create policy exists, and creates it if necessary.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    protected virtual async Task CheckCreatePolicyAsync()
    {
        await CheckPolicyAsync(CreatePolicyName);
    }

    /// <summary>
    /// Checks if the update policy exists, and creates it if necessary.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    protected virtual async Task CheckUpdatePolicyAsync()
    {
        await CheckPolicyAsync(UpdatePolicyName);
    }

    /// <summary>
    /// Checks if the delete policy exists, and creates it if necessary.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    protected virtual async Task CheckDeletePolicyAsync()
    {
        await CheckPolicyAsync(DeletePolicyName);
    }

    /// <summary>
    /// Maps <typeparamref name="TCreateInput"/> to <typeparamref name="TEntity"/> to create a new entity.
    /// It uses <see cref="MapToEntity(TCreateInput)"/> by default.
    /// It can be overriden for custom mapping.
    /// Overriding this has higher priority than overriding the <see cref="MapToEntity(TCreateInput)"/>
    /// </summary>
    protected virtual Task<TEntity> CreateMapToEntityAsync(TCreateInput createInput)
    {
        return Task.FromResult(MapToEntity(createInput));
    }

    /// <summary>
    /// Maps <typeparamref name="TCreateInput"/> to <typeparamref name="TEntity"/> to create a new entity.
    /// It uses <see cref="IObjectMapper"/> by default.
    /// It can be overriden for custom mapping.
    /// </summary>
    protected virtual TEntity MapToEntity(TCreateInput createInput)
    {
        var entity = ObjectMapper.Map<TCreateInput, TEntity>(createInput);
        SetIdForGuids(entity);
        return entity;
    }

    /// <summary>
    /// Sets Id value for the entity if <typeparamref name="TKey"/> is <see cref="Guid"/>.
    /// It's used while creating a new entity.
    /// </summary>
    protected virtual void SetIdForGuids(TEntity entity)
    {
        if (entity is IEntity<Guid> entityWithGuidId && entityWithGuidId.Id == Guid.Empty)
        {
            EntityHelper.TrySetId(
                entityWithGuidId,
                () => GuidGenerator.Create(),
                true
            );
        }
    }

    /// <summary>
    /// Maps <typeparamref name="TUpdateInput"/> to <typeparamref name="TEntity"/> to update the entity.
    /// It uses <see cref="MapToEntity(TUpdateInput, TEntity)"/> by default.
    /// It can be overriden for custom mapping.
    /// Overriding this has higher priority than overriding the <see cref="MapToEntity(TUpdateInput, TEntity)"/>
    /// </summary>
    protected virtual Task UpdateInputMapToEntityAsync(TUpdateInput updateInput, TEntity entity)
    {
        MapToEntity(updateInput, entity);
        return Task.CompletedTask;
    }

    /// <summary>
    /// Maps <typeparamref name="TUpdateInput"/> to <typeparamref name="TEntity"/> to update the entity.
    /// It uses <see cref="IObjectMapper"/> by default.
    /// It can be overriden for custom mapping.
    /// </summary>
    protected virtual void MapToEntity(TUpdateInput updateInput, TEntity entity)
    {
        ObjectMapper.Map(updateInput, entity);
    }

    /// <summary>
    /// Tries to set the tenant ID for the entity.
    /// </summary>
    /// <param name="entity">The entity to set the tenant ID for.</param>
    protected virtual void TryToSetTenantId(TEntity entity)
    {
        if (entity is IMultiTenant && HasTenantIdProperty(entity))
        {
            var tenantId = CurrentTenant.Id;

            if (!tenantId.HasValue)
            {
                return;
            }

            var propertyInfo = entity.GetType().GetProperty(nameof(IMultiTenant.TenantId));

            if (propertyInfo == null || propertyInfo.GetSetMethod(true) == null)
            {
                return;
            }

            propertyInfo.SetValue(entity, tenantId);
        }
    }
    /// <summary>
    /// Checks if the entity has a TenantId property.
    /// </summary>
    /// <param name="entity">The entity to check.</param>
    /// <returns>True if the entity has a TenantId property; otherwise, false.</returns>
    protected virtual bool HasTenantIdProperty(TEntity entity)
    {
        return entity.GetType().GetProperty(nameof(IMultiTenant.TenantId)) != null;
    }

    protected virtual async Task<TEntity> InsertAsync(TEntity entity)
    {
        return await Repository.InsertAsync(entity, autoSave: true);
    }
    protected virtual async Task<TEntity> UpdateAsync(TEntity entity)
    {
        return await Repository.UpdateAsync(entity, autoSave: true);
    }
}
