using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nels.Abp.SysMng.Files;
using Nels.Aigc.Dtos;
using Nels.Aigc.Entities;
using Nels.Aigc.Knowledges;
using Nels.Aigc.Permissions;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.BlobStoring;
using Volo.Abp.Domain.Repositories;

namespace Nels.Aigc.Services;


[Route(AigcRemoteServiceConsts.knowledgeDocumentRoute)]
public class KnowledgeDocumentAppService(IRepository<KnowledgeDocument, Guid> repository
    , IRepository<FileEntity, Guid> fileRepository
    , IBackgroundJobManager backgroundJobManager
    ) : AigcAppService
{
    private readonly IRepository<KnowledgeDocument> _repository = repository;
    private readonly IRepository<FileEntity, Guid> _fileRepository = fileRepository;
    private readonly IBackgroundJobManager _backgroundJobManager = backgroundJobManager;


    [HttpPost]
    [Route("[action]")]
    [Authorize(Policy = AigcPermissions.KnowledgeDocument.Create)]
    public virtual async Task<KnowledgeDocumentDto> AddKnowledgeDocumentAsync(AddKnowledgeDocumentRequest request)
    {
        var file = await _fileRepository.GetAsync(request.FileId) ?? throw new BusinessException();

        var entity = new KnowledgeDocument(GuidGenerator.Create(), file.Name, file.Type, file.Id);

        await _repository.InsertAsync(entity);
        await _backgroundJobManager.EnqueueAsync(new DocumentSplitArgs { KnowledgeDocumentId = entity.Id, FileId = file.Id });

        return ObjectMapper.Map<KnowledgeDocument, KnowledgeDocumentDto>(entity);
    }
}
