using Microsoft.AspNetCore.Mvc;
using Nels.Abp.Ddd.Application.Contracts;
using Nels.Abp.Ddd.Application.Services;
using Nels.Aigc.Dtos;
using Nels.Aigc.Entities;
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


[Route(AigcRemoteServiceConsts.modelRoute)]
public class ModelAppService(IRepository<Model, Guid> repository) : RouteCrudGetAllAppService<Model, ModelDto, ModelGetListOutputDto, Guid, ModelGetListInputDto, ModelDto, ModelDto>(repository), IModelService
{

    [HttpPost]
    [Route("[action]")]
    public virtual async Task ModelSettingAsync(ModelSettingDto settingDto)
    {
        if (settingDto == null) throw new BusinessException(nameof(settingDto));

        List<EnumDto<ModelProvider>> providers = EnumExtensions.ToEnumDtoList<ModelProvider>();
        if (!providers.Any(x => x.Value == settingDto.Provider)) throw new BusinessException(nameof(settingDto.Provider));

        List<Model> entities = await repository.GetListAsync(x => x.Provider == settingDto.Provider);
        foreach (Model model in entities)
        {
            if (!string.IsNullOrWhiteSpace(settingDto.Endpoint))
                model.Endpoint = settingDto.Endpoint;
            if (!string.IsNullOrWhiteSpace(settingDto.AccessKey))
                model.AccessKey = settingDto.AccessKey;
            if (!string.IsNullOrWhiteSpace(settingDto.SecretKey))
                model.SecretKey = settingDto.SecretKey;
            if (!string.IsNullOrWhiteSpace(settingDto.DeploymentName))
                model.DeploymentName = settingDto.DeploymentName;

            model.IsEnabled = true;
        }
        await repository.UpdateManyAsync(entities);
    }

    [RemoteService(false)]
    public async Task<List<IModel>> GetAllModels()
    {
        var entities = await Repository.GetListAsync();
        var entityDtos = MapList<Model, ModelDto>(entities);

        return [.. entityDtos];
    }

    [HttpPost]
    [Route("[action]")]
    public async Task SetIsDefaultAsync(Guid id)
    {
        //var entity = await Repository.GetAsync(id);
        //if (entity == null) return;

        //var entites = await Repository.GetListAsync(x => x.Type == entity.Type && x.IsDefault);
        //entites.ForEach(x => { x.IsDefault = false; });

        //entity.IsDefault = true;
        //entites.Add(entity);

        //await Repository.UpdateManyAsync(entites);
    }
    [HttpPost]
    [Route("[action]")]
    public async Task SetKeyAsync(ModelSetKeyDto data)
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
}
