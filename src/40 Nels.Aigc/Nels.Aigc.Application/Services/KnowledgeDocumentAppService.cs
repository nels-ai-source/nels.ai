using Microsoft.AspNetCore.Mvc;
using Nels.Abp.SysMng.Files;
using Nels.Aigc.Dtos;
using Nels.Aigc.Entities;
using Nels.Aigc.Knowledges;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Domain.Repositories;

namespace Nels.Aigc.Services;


[Route(AigcRemoteServiceConsts.knowledgeDocumentRoute)]
public class KnowledgeDocumentAppService(IRepository<KnowledgeDocument, Guid> repository
    , IRepository<FileEntity, Guid> fileRepository
    , IRepository<KnowledgeDocumentParagraph> paragraphRepository
    , IBackgroundJobManager backgroundJobManager
    , KnowledgeDocumentDomainService knowledgeDocumentDomainService,
      KnowledgeDomainService knowledgeDomainService) : AigcAppService
{
    private readonly IRepository<KnowledgeDocument> _repository = repository;
    private readonly IRepository<KnowledgeDocumentParagraph> _paragraphRepository = paragraphRepository;
    private readonly IRepository<FileEntity, Guid> _fileRepository = fileRepository;
    private readonly IBackgroundJobManager _backgroundJobManager = backgroundJobManager;

    private readonly KnowledgeDocumentDomainService _knowledgeDocumentDomainService = knowledgeDocumentDomainService;
    private readonly KnowledgeDomainService _knowledgeDomainService = knowledgeDomainService;

    [HttpPost]
    [Route("[action]")]
    public virtual async Task<KnowledgeDocumentDto> CreateAsync(AddKnowledgeDocumentRequest request)
    {
        var file = await _fileRepository.GetAsync(request.FileId) ?? throw new BusinessException();

        var entity = new KnowledgeDocument(GuidGenerator.Create(), request.KnowledgeId, file.Name, file.Type, file.Id);

        await _repository.InsertAsync(entity, true);
        await _knowledgeDocumentDomainService.DocumentSplitAsync(new DocumentSplitArgs { FileId = file.Id, KnowledgeDocumentId = entity.Id, MaxTokensPerParagraph = request.MaxTokensPerParagraph });
        await _knowledgeDomainService.UpdateKnowledgeAsync(entity.KnowledgeId);

        return ObjectMapper.Map<KnowledgeDocument, KnowledgeDocumentDto>(entity);
    }

    [HttpPost]
    [Route("[action]")]
    public virtual async Task UpdateAsync(Guid knowledgeDocumentId, string name)
    {
        var entity = await _repository.GetAsync(x => x.Id == knowledgeDocumentId);
        entity.Name = name;
        await _repository.UpdateAsync(entity);
    }

    [HttpPost]
    [Route("[action]")]
    public virtual async Task DeleteAsync(Guid knowledgeDocumentId)
    {
        var entity = await _repository.GetAsync(x => x.Id == knowledgeDocumentId);
        await _repository.DeleteAsync(entity, true);
        await _knowledgeDomainService.UpdateKnowledgeAsync(entity.KnowledgeId);
    }

    [HttpPost]
    [Route("[action]")]
    public virtual async Task<List<KnowledgeDocumentDto>> GetListAsync(Guid knowledgeId)
    {
        var entities = await _repository.GetListAsync(x => x.KnowledgeId == knowledgeId);
        return ObjectMapper.Map<List<KnowledgeDocument>, List<KnowledgeDocumentDto>>([.. entities.OrderBy(x => x.CreationTime)]);
    }

    [HttpPost]
    [Route("[action]")]
    public virtual async Task<List<KnowledgeDocumentParagraphDto>> GetParagraphListAsync(Guid knowledgeDocumentId)
    {
        var entities = await _paragraphRepository.GetListAsync(x => x.KnowledgeDocumentId == knowledgeDocumentId);
        return ObjectMapper.Map<List<KnowledgeDocumentParagraph>, List<KnowledgeDocumentParagraphDto>>([.. entities.OrderBy(x => x.Index)]);
    }

    [HttpPost]
    [Route("[action]")]
    public virtual async Task UpdateParagraphAsync(UpdateKnowledgeDocumentParagraphDto dto)
    {
        var entity = await _paragraphRepository.GetAsync(x => x.Id == dto.Id);
        entity.Content = dto.Content;
        await _paragraphRepository.UpdateAsync(entity);
    }
}
