using Microsoft.AspNetCore.Mvc;
using Nels.Abp.Ddd.Application.Services;
using Nels.Aigc.Dtos;
using Nels.Aigc.Entities;
using Nels.Aigc.Permissions;
using System;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Nels.Aigc.Services;


[Route(AigcRemoteServiceConsts.knowledgeRoute)]
public class KnowledgeAppService : RouteCrudGetAllAppService<Knowledge, KnowledgeDto, Guid>
{
    public KnowledgeAppService(IRepository<Knowledge, Guid> repository) : base(repository)
    {
        CreatePolicyName = AigcPermissions.Knowledge.Create;
        UpdatePolicyName = AigcPermissions.Knowledge.Update;
        DeletePolicyName = AigcPermissions.Knowledge.Delete;
        GetPolicyName = AigcPermissions.Knowledge.GetList;
        GetListPolicyName = AigcPermissions.Knowledge.GetList;
    }

    public override Task DeleteAsync(Guid id)
    {
        return base.DeleteAsync(id);
    }

}
