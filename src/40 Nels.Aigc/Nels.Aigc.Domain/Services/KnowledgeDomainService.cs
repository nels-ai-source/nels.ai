using Nels.Abp.Ddd.Domain.Services;
using Nels.Aigc.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;

namespace Nels.Aigc.Services;

public class KnowledgeDomainService(IRepository<Knowledge, Guid> repository
    , IRepository<KnowledgeDocument, Guid> knowledgeDocumentRepository
    , IRepository<KnowledgeDocumentParagraph, Guid> paragraphRepository
    ) : DomainService
{
    private readonly IRepository<Knowledge, Guid> _repository = repository;
    private readonly IRepository<KnowledgeDocument, Guid> _knowledgeDocumentRepository = knowledgeDocumentRepository;
    private readonly IRepository<KnowledgeDocumentParagraph, Guid> _paragraphRepository = paragraphRepository;

    public virtual async Task UpdateKnowledgeAsync(Guid knowledgeId)
    {
        var knowledge = await _repository.GetAsync(knowledgeId) ?? throw new BusinessException();
        var documents = await _knowledgeDocumentRepository.GetListAsync(x => x.KnowledgeId == knowledgeId) ?? throw new BusinessException();

        knowledge.DocumentCount = documents.Count;
        knowledge.Length = documents.Sum(_ => _.Length);
        knowledge.RetrievalCount = documents.Sum(_ => _.RetrievalCount);

        await _repository.UpdateAsync(knowledge);
    }
}
