using Microsoft.AspNetCore.Mvc;
using Nels.Abp.Ddd.Application.Contracts;
using Nels.Abp.Ddd.Application.Services;
using Nels.Aigc.Dtos;
using Nels.Aigc.Entities;
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
public class ModelAppService(IRepository<Model, Guid> repository) : RouteCrudGetAllAppService<Model, ModelDto, ModelGetListOutputDto, Guid, ModelGetListInputDto, ModelDto, ModelDto>(repository)
{
    protected override Task<List<ModelGetListOutputDto>> MapToGetListOutputDtosAsync(List<Model> entities)
    {
        List<ModelGetListOutputDto> outputs = [];
        List<ModelProvider> modelProviders = entities.Select(x => x.Provider).Distinct().ToList();
        List<EnumDto<ModelProvider>> providers = EnumExtensions.ToEnumDtoList<ModelProvider>().Where(x => modelProviders.Contains(x.Value)).ToList();

        return Task.FromResult(providers.Select(x => new ModelGetListOutputDto
        {
            Id = x.Id,
            Name = x.Label,
            Provider = x.Value,
            children = entities.Where(model => model.Provider == x.Value).Select(model => new ModelGetListOutputDto
            {
                Id = model.Id,
                Name = model.Name,
                Endpoint = model.Endpoint,
                Properties = model.Properties,
                ParentId = x.Id,
                Provider = x.Value,
                Type = model.Type,
                IsEnabled = model.IsEnabled,
                ModelCapabilities = model.ModelCapabilities
            }).ToList(),
        }).ToList());
    }

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
}
