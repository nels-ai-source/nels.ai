using Microsoft.AspNetCore.Mvc;
using Nels.Abp.Ddd.Application.Services;
using Nels.Aigc.Dtos;
using Nels.Aigc.Entities;
using Nels.Aigc.Permissions;
using System;
using Volo.Abp.Domain.Repositories;

namespace Nels.Aigc.Services;


[Route(AigcRemoteServiceConsts.knowledgeDocumentRoute)]
public class KnowledgeDocumentAppService : RouteCrudGetAllAppService<KnowledgeDocument, KnowledgeDocumentDto, Guid>
{
    public KnowledgeDocumentAppService(IRepository<KnowledgeDocument, Guid> repository) : base(repository)
    {
        CreatePolicyName = AigcPermissions.KnowledgeDocument.Create;
        UpdatePolicyName = AigcPermissions.KnowledgeDocument.Update;
        DeletePolicyName = AigcPermissions.KnowledgeDocument.Delete;
        GetPolicyName = AigcPermissions.KnowledgeDocument.GetList;
        GetListPolicyName = AigcPermissions.KnowledgeDocument.GetList;
    }

}
