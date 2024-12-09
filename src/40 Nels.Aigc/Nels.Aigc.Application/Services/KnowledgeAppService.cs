using Microsoft.AspNetCore.Mvc;
using Microsoft.SemanticKernel.Memory;
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
    private readonly IMemoryStore _memoryStore;
    public KnowledgeAppService(IRepository<Knowledge, Guid> repository, IMemoryStore memoryStore) : base(repository)
    {
        CreatePolicyName = AigcPermissions.Knowledge.Create;
        UpdatePolicyName = AigcPermissions.Knowledge.Update;
        DeletePolicyName = AigcPermissions.Knowledge.Delete;
        GetPolicyName = AigcPermissions.Knowledge.GetList;
        GetListPolicyName = AigcPermissions.Knowledge.GetList;
        _memoryStore = memoryStore;
    }
    protected override async Task<Knowledge> BeforeInsertAsync(Knowledge entity)
    {
        await _memoryStore.CreateCollectionAsync(entity.Id.ToString());
        return await Task.FromResult(entity);
    }
    protected override async Task AfterDeleteAsync(Knowledge entity)
    {
        await _memoryStore.DeleteCollectionAsync(entity.Id.ToString());
    }
}
