using Microsoft.AspNetCore.Mvc;
using Nels.Abp.Ddd.Application.Services;
using Nels.Aigc.Dtos;
using Nels.Aigc.Entities;
using Nels.Aigc.Permissions;
using Nels.SemanticKernel;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;

namespace Nels.Aigc.Services;


[Route(AigcRemoteServiceConsts.promptRoute)]
public class PromptAppService : RouteCrudGetAllAppService<Prompt, PromptDto, Guid>, IPromptService
{
    public PromptAppService(IRepository<Prompt, Guid> repository) : base(repository)
    {
        CreatePolicyName = AigcPermissions.Prompt.Create;
        UpdatePolicyName = AigcPermissions.Prompt.Update;
        DeletePolicyName = AigcPermissions.Prompt.Delete;
        GetPolicyName = AigcPermissions.Prompt.GetList;
        GetListPolicyName = AigcPermissions.Prompt.GetList;
    }

    [RemoteService(false)]
    public virtual async Task<IPrompt> Get(Guid id)
    {
        var entity = await Repository.GetAsync(x => x.Id == id);
        return MapToGetListOutputDto(entity);
    }
}
