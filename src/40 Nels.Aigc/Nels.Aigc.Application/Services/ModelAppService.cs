using Microsoft.AspNetCore.Mvc;
using Nels.Abp.Ddd.Application.Services;
using Nels.Aigc.Dtos;
using Nels.Aigc.Entities;
using Nels.SemanticKernel.Enums;
using Nels.SemanticKernel.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Nels.Aigc.Services;


[Route(AigcRemoteServiceConsts.modelRoute)]
public class ModelAppService : RouteCrudGetAllAppService<Model, ModelDto, ModelGetListOutputDto, Guid, ModelGetListInputDto, ModelDto, ModelDto>
{
    public ModelAppService(IRepository<Model, Guid> repository) : base(repository)
    {
        //CreatePolicyName = AigcPermissions.Model.Create;
        //UpdatePolicyName = AigcPermissions.Model.Update;
        //DeletePolicyName = AigcPermissions.Model.Delete;
        //GetPolicyName = AigcPermissions.Model.GetList;
        //GetListPolicyName = AigcPermissions.Model.GetList;
    }

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
            }).ToList(),
        }).ToList());
    }
}
