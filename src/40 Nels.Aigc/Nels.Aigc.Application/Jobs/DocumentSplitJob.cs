using Nels.Aigc.Knowledges;
using Nels.Aigc.Services;
using System.Threading.Tasks;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.DependencyInjection;

namespace Nels.Aigc.Jobs;

public class DocumentSplitJob(KnowledgeDocumentDomainService knowledgeDocumentDomainService) : AsyncBackgroundJob<DocumentSplitArgs>, ITransientDependency
{
    private readonly KnowledgeDocumentDomainService _knowledgeDocumentDomainService = knowledgeDocumentDomainService;

    public override async Task ExecuteAsync(DocumentSplitArgs args)
    {
        await _knowledgeDocumentDomainService.DocumentSplitAsync(args);
    }
}

