using Microsoft.AspNetCore.Mvc;
using Nels.Abp.Ddd.Application.Services;
using Nels.Aigc.Dtos;
using Nels.Aigc.Entities;
using Nels.Aigc.Permissions;
using Nels.SemanticKernel;
using Nels.SemanticKernel.Enums;
using Nels.SemanticKernel.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;

namespace Nels.Aigc.Services;


[Route(AigcRemoteServiceConsts.modelInstanceRoute)]
public class ModelInstanceAppService : RouteCrudGetAllAppService<ModelInstance, ModelInstanceDto, ModelInstanceGetListOutputDto, Guid, ModelInstanceGetListInputDto, ModelInstanceCreateInputDto, ModelInstanceUpdateInputDto>, IModelInstanceService
{
    private readonly IRepository<Model, Guid> _modelRepository;
    public ModelInstanceAppService(IRepository<ModelInstance, Guid> repository, IRepository<Model, Guid> modelRepository) : base(repository)
    {
        //CreatePolicyName = AigcPermissions.ModelInstance.Create;
        //UpdatePolicyName = AigcPermissions.ModelInstance.Update;
        //DeletePolicyName = AigcPermissions.ModelInstance.Delete;
        //GetPolicyName = AigcPermissions.ModelInstance.GetList;
        //GetListPolicyName = AigcPermissions.ModelInstance.GetList;
        _modelRepository = modelRepository;
    }
    [RemoteService(false)]
    public async Task<List<IModelInstance>> GetAllInstances()
    {
        var entities = await Repository.GetListAsync();
        var entityDtos = MapList<ModelInstance, ModelInstanceDto>(entities);

        return [.. entityDtos];
    }

    [HttpPost]
    [Route("[action]")]
    public async Task SetIsDefaultAsync(Guid id)
    {
        var entity = await Repository.GetAsync(id);
        if (entity == null) return;

        var entites = await Repository.GetListAsync(x => x.Type == entity.Type && x.IsDefault);
        entites.ForEach(x => { x.IsDefault = false; });

        entity.IsDefault = true;
        entites.Add(entity);

        await Repository.UpdateManyAsync(entites);
    }

    [HttpPost]
    [Route("[action]")]
    public async Task SetKeyAsync(ModelInstanceSetKeyDto data)
    {
        if (data == null || data?.Ids.Count == 0) return;

        var entities = await Repository.GetListAsync(x => data.Ids.Contains(x.Id));
        if (entities == null || entities?.Count == 0) return;

        entities.ForEach(entity =>
        {
            entity.AccessKey = data?.AccessKey ?? entity.AccessKey;
            entity.SecretKey = data?.SecretKey ?? entity.SecretKey;
        });
        await Repository.UpdateManyAsync(entities);
    }

    protected override async Task<IQueryable<ModelInstance>> CreateFilteredQueryAsync(ModelInstanceGetListInputDto? input)
    {
        return (await Repository.GetQueryableAsync())
             .WhereIf(!string.IsNullOrWhiteSpace(input?.Name), x => x.Name.Contains(input.Name) || x.DeploymentName.Contains(input.Name))
             .WhereIf(input.ModelProvider != null, x => x.Provider == input.ModelProvider)
             .WhereIf(input.ModelId != null, x => x.ModelId == input.ModelId);
    }

    protected override async Task<ModelInstance> CreateMapToEntityAsync(ModelInstanceCreateInputDto createInput)
    {
        var model = await _modelRepository.GetAsync(createInput.ModelId) ?? throw new ArgumentNullException(nameof(createInput.ModelId));

        var entity = ObjectMapper.Map<Model, ModelInstance>(model);
        entity.ModelId = createInput.ModelId;
        entity.DeploymentName = createInput.DeploymentName ?? entity.DeploymentName;
        entity.AccessKey = createInput.AccessKey ?? entity.AccessKey;
        entity.SecretKey = createInput.SecretKey ?? entity.SecretKey;
        entity.Endpoint = createInput.Endpoint ?? entity.Endpoint;
        entity.Description = createInput.Description ?? entity.Description;

        return entity;
    }

    protected override Task<List<ModelInstanceGetListOutputDto>> MapToGetListOutputDtosAsync(List<ModelInstance> entities)
    {
        List<ModelInstanceGetListOutputDto> outputs = [];
        Dictionary<ModelProvider, string> providers = EnumExtensions.ToDictionary<ModelProvider>();
        Dictionary<ModelType, string> modelTypes = EnumExtensions.ToDictionary<ModelType>();

        return Task.FromResult(entities.Select(x => new ModelInstanceGetListOutputDto
        {
            Id = x.Id,
            Name = x.Name,
            Provider = x.Provider,
            ProviderName = providers[x.Provider],
            Type = x.Type,
            TypeName = modelTypes[x.Type],
            DeploymentName = x.DeploymentName,
            IsDefault = x.IsDefault,
            Endpoint = x.Endpoint,
            Description = x.Description
        }).ToList());
    }
}
