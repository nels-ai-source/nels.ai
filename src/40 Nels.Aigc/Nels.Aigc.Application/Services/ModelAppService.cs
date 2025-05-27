using Microsoft.AspNetCore.Mvc;
using Nels.Abp.Ddd.Application.Services;
using Nels.Aigc.Dtos;
using Nels.SemanticKernel;
using Nels.SemanticKernel.Enums;
using Nels.SemanticKernel.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Model = Nels.Aigc.Entities.Model;

namespace Nels.Aigc.Services;


[Route(AigcRemoteServiceConsts.modelRoute)]
public class ModelAppService(IRepository<Model, Guid> repository) : RouteCrudGetAllAppService<Model, ModelDto, ModelGetListOutputDto, Guid, ModelGetListInputDto, ModelDto, ModelUpdateInputDto>(repository), IModelService
{
    [HttpPost]
    [Route("[action]")]
    public async Task SetIsEnabledAsync(Guid id, bool isEnabled)
    {

        var entity = await Repository.GetAsync(id);
        if (entity == null) return;

        entity.IsEnabled = isEnabled;

        await Repository.UpdateAsync(entity);
    }

    [HttpPost]
    [Route("[action]")]
    public async Task SetKeyAsync(ModelSetKeyDto data)
    {
        if (data == null || (data.Provider == null && data.Id == null)) return;

        var query = (await Repository.GetQueryableAsync())
            .WhereIf(data.Id != null, x => x.Id == data.Id)
            .WhereIf(data.Provider != null, x => x.Provider == data.Provider);

        var entities = query.ToList();
        if (entities == null || entities?.Count == 0) return;

        entities.ForEach(entity =>
        {
            entity.AccessKey = data?.AccessKey ?? entity.AccessKey;
            entity.SecretKey = data?.SecretKey ?? entity.SecretKey;
        });
        await Repository.UpdateManyAsync(entities);
    }

    [HttpPost]
    [Route("[action]")]
    public async Task CreateListAsync(List<ModelDto> dtos)
    {
        await CheckCreatePolicyAsync();

        List<Model> entities = MapList<ModelDto, Model>(dtos);

        foreach (var entity in entities)
        {
            TryToSetTenantId(entity);

            await ProcessCreate(entity);

            await ValidateCreate(entity);
        }

        await Repository.InsertManyAsync(entities);
    }

    protected override async Task<IQueryable<Model>> CreateFilteredQueryAsync(ModelGetListInputDto? input)
    {
        var query = await base.CreateFilteredQueryAsync(input);
        if (input == null) return query;
        List<string> modelCapabilities = input?.ModelCapabilities?.Select(x => $"[{(int)x}]").ToList() ?? [];

        return query.WhereIf(!string.IsNullOrWhiteSpace(input.Keyword), x => x.Name.Contains(input.Keyword))
            .WhereIf(input.Provider != null, x => x.Provider == input.Provider)
            .WhereIf(input.Type != null, x => x.Type == input.Type)
            .WhereIf(input.MaxTokens != null, x => x.MaxTokens >= input.MaxTokens)
            .WhereIf(modelCapabilities.Count > 0, x => x.Capabilities != null && modelCapabilities.All(cap => x.Capabilities.Contains(cap)));
    }

    protected override IQueryable<Model> ApplySorting(IQueryable<Model> query, ModelGetListInputDto? input)
    {
        return query.OrderBy(x => x.Provider).ThenBy(x => x.Name);
    }

    protected override async Task ValidateCreate(Model entity)
    {
        if (await Repository.AnyAsync(x => x.Name == entity.Name && x.Id != entity.Id))
        {
            throw new BusinessException(code: "001", message: $"Model name {entity.Name} already exists.");
        }

        await base.ValidateCreate(entity);
    }

    [RemoteService(false)]
    public async Task<List<IModel>> GetAllModels()
    {
        var entities = await Repository.GetListAsync();
        var entityDtos = MapList<Model, ModelDto>(entities);

        return [.. entityDtos];
    }
}
