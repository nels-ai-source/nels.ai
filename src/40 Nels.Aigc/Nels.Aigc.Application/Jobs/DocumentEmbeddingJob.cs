using Nels.Aigc.Knowledges;
using Nels.Aigc.Services;
using System.Threading.Tasks;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.DependencyInjection;

namespace Nels.Aigc.Jobs;

public class DocumentEmbeddingJob(KnowledgeDocumentDomainService knowledgeDocumentDomainService) : AsyncBackgroundJob<DocumentEmbeddingArgs>, ITransientDependency
{
    private readonly KnowledgeDocumentDomainService _knowledgeDocumentDomainService = knowledgeDocumentDomainService;

    public override async Task ExecuteAsync(DocumentEmbeddingArgs args)
    {
        await _knowledgeDocumentDomainService.DocumentEmbeddingAsync(args);
    }
}

