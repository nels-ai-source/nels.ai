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
    private IRepository<KnowledgeDocument, Guid> DocumentRepository { get; set; }
    public KnowledgeAppService(IRepository<Knowledge, Guid> repository, IRepository<KnowledgeDocument, Guid> documentRepository) : base(repository)
    {
        CreatePolicyName = AigcPermissions.Knowledge.Create;
        UpdatePolicyName = AigcPermissions.Knowledge.Update;
        DeletePolicyName = AigcPermissions.Knowledge.Delete;
        GetPolicyName = AigcPermissions.Knowledge.GetList;
        GetListPolicyName = AigcPermissions.Knowledge.GetList;

        DocumentRepository = documentRepository;
    }

    protected override KnowledgeDto MapToGetOutputDto(Knowledge entity)
    {
        var dto = base.MapToGetOutputDto(entity);
        var documents = DocumentRepository.GetListAsync(x => x.KnowledgeId == entity.Id).GetAwaiter().GetResult();

        dto.Documents = MapList<KnowledgeDocument, KnowledgeDocumentDto>(documents);

        return dto;
    }


}
