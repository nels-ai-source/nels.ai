using Microsoft.Extensions.Configuration;
using Microsoft.KernelMemory.DataFormats;
using Microsoft.KernelMemory.DataFormats.Text;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.Postgres;
using Microsoft.SemanticKernel.Embeddings;
using Microsoft.SemanticKernel.Memory;
using Nels.Abp.SysMng.Files;
using Nels.Aigc.Entities;
using Nels.Aigc.Knowledges;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.BlobStoring;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace Nels.Aigc.Services;

public class KnowledgeDocumentDomainService(IRepository<FileEntity, Guid> fileRepository
    , IRepository<KnowledgeDocument, Guid> repository
    , IRepository<KnowledgeDocumentParagraph, Guid> paragraphRepository
    , IRepository<Knowledge, Guid> knowledgeRepository
    , IBlobContainer blobContainer
    , Kernel kernel
    , IBackgroundJobManager backgroundJobManager
    , IConfiguration configuration
    , IEnumerable<IContentDecoder> decoders) : DomainService
{
    private readonly IRepository<FileEntity, Guid> _fileRepository = fileRepository;
    private readonly IRepository<KnowledgeDocument, Guid> _repository = repository;
    private readonly IRepository<Knowledge, Guid> _knowledgeRepository = knowledgeRepository;
    private readonly IRepository<KnowledgeDocumentParagraph, Guid> _paragraphRepository = paragraphRepository;
    private readonly IBlobContainer _blobContainer = blobContainer;
    private readonly IEnumerable<IContentDecoder> _decoders = decoders;
    private readonly IBackgroundJobManager _backgroundJobManager = backgroundJobManager;
    private readonly IConfiguration _configuration = configuration;

    private readonly Kernel _kernel = kernel;
    public virtual async Task DocumentSplitAsync(DocumentSplitArgs args)
    {
        var file = await _fileRepository.GetAsync(args.FileId) ?? throw new BusinessException();
        var document = await _repository.GetAsync(args.KnowledgeDocumentId) ?? throw new BusinessException();

        var stream = await _blobContainer.GetAsync(file.Path);

        var decoder = _decoders.LastOrDefault(d => d.SupportsMimeType(file.MimeType));
        if (decoder is not null)
        {
            var fileContent = await decoder.DecodeAsync(stream).ConfigureAwait(false);
            var lines = fileContent.Sections.Select(x => x.Content).ToList();
            var paragraphs = TextChunker.SplitPlainTextParagraphs(lines, args.MaxTokensPerParagraph);
            var index = 0;
            foreach (var content in paragraphs)
            {
                document.AddParagraph(GuidGenerator.Create(), index++, content);
            }

            await _paragraphRepository.InsertManyAsync(document.KnowledgeDocumentParagraphs);
            await _repository.UpdateAsync(document);

            await _backgroundJobManager.EnqueueAsync(new DocumentEmbeddingArgs { KnowledgeId = document.KnowledgeId, KnowledgeDocumentId = document.Id });
        }
    }

    public virtual async Task DocumentEmbeddingAsync(DocumentEmbeddingArgs args)
    {
        var konwlledge = await _knowledgeRepository.GetAsync(args.KnowledgeId) ?? throw new BusinessException();
        var paragraphs = await _paragraphRepository.GetListAsync(x => x.KnowledgeDocumentId == args.KnowledgeDocumentId && x.Embedding == false);
        var connectionString = _configuration.GetConnectionString(AigcDbProperties.ConnectionStringName) ?? throw new BusinessException();

        IMemoryStore store = new PostgresMemoryStore(connectionString, 1536);
        ITextEmbeddingGenerationService embeddingGenerator = _kernel.GetRequiredService<ITextEmbeddingGenerationService>();

        var _textMemory = new SemanticTextMemory(store, embeddingGenerator);

        foreach (var paragraph in paragraphs)
        {
            await _textMemory.SaveInformationAsync($"kn_{args.KnowledgeDocumentId}", paragraph.Content, paragraph.Id.ToString());
            paragraph.Embedding = true;
        }
        await _paragraphRepository.UpdateManyAsync(paragraphs);
    }
}
