using AutoMapper.Internal.Mappers;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nels.Abp.Ddd.Application.Services;
using Nels.Abp.SysMng.Files;
using Nels.Aigc.Dtos;
using Nels.Aigc.Entities;
using Nels.Aigc.Knowledges;
using Nels.Aigc.Permissions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.BlobStoring;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;
using static Nels.Aigc.Permissions.AigcPermissions;

namespace Nels.Aigc.Services;


[Route(AigcRemoteServiceConsts.knowledgeDocumentRoute)]
public class KnowledgeDocumentAppService : AigcAppService
{
    private readonly IRepository<Entities.KnowledgeDocument> _repository;
    private readonly IRepository<KnowledgeDocumentParagraph> _paragraphRepository;
    private readonly IRepository<FileEntity, Guid> _fileRepository;
    private readonly IBackgroundJobManager _backgroundJobManager;

    private readonly KnowledgeDocumentDomainService _knowledgeDocumentDomainService;

    public KnowledgeDocumentAppService(IRepository<Entities.KnowledgeDocument, Guid> repository
    , IRepository<FileEntity, Guid> fileRepository
    , IRepository<KnowledgeDocumentParagraph> paragraphRepository
    , IBackgroundJobManager backgroundJobManager
    , KnowledgeDocumentDomainService knowledgeDocumentDomainService)
    {
        _repository = repository;
        _paragraphRepository = paragraphRepository;
        _fileRepository = fileRepository;
        _backgroundJobManager = backgroundJobManager;

        _knowledgeDocumentDomainService = knowledgeDocumentDomainService;
    }

    [HttpPost]
    [Route("[action]")]
    //[Authorize(Policy = AigcPermissions.KnowledgeDocument.Create)]
    public virtual async Task<KnowledgeDocumentDto> AddKnowledgeDocumentAsync(AddKnowledgeDocumentRequest request)
    {
        var file = await _fileRepository.GetAsync(request.FileId) ?? throw new BusinessException();

        var entity = new Entities.KnowledgeDocument(GuidGenerator.Create(), request.KnowledgeId, file.Name, file.Type, file.Id);

        await _repository.InsertAsync(entity, true);
        await _knowledgeDocumentDomainService.DocumentSplitAsync(new DocumentSplitArgs { FileId = file.Id, KnowledgeDocumentId = entity.Id, MaxTokensPerParagraph = request.MaxTokensPerParagraph });

        return ObjectMapper.Map<Entities.KnowledgeDocument, KnowledgeDocumentDto>(entity);
    }

    [HttpPost]
    [Route("[action]")]
    public virtual async Task UodateKnowledgeDocumentNameAsync(Guid knowledgeDocumentId, string name)
    {
        var entity = await _repository.GetAsync(x => x.Id == knowledgeDocumentId);
        entity.Name = name;
        await _repository.UpdateAsync(entity);
    }

    [HttpPost]
    [Route("[action]")]
    public virtual async Task<List<KnowledgeDocumentDto>> GetListAsync(Guid knowledgeId)
    {
        var entities = await _repository.GetListAsync(x => x.KnowledgeId == knowledgeId);
        return ObjectMapper.Map<List<Entities.KnowledgeDocument>, List<KnowledgeDocumentDto>>([.. entities.OrderBy(x => x.CreationTime)]);
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
    public virtual async Task UpdateKnowledgeDocumentParagraphAsync(UpdateKnowledgeDocumentParagraphDto dto)
    {
        var entity = await _paragraphRepository.GetAsync(x => x.Id == dto.Id);
        entity.Content = dto.Content;
        await _paragraphRepository.UpdateAsync(entity);
    }
}
