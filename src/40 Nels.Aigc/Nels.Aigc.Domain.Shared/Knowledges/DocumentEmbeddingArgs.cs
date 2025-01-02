using System;

namespace Nels.Aigc.Knowledges;

public class DocumentEmbeddingArgs
{
    public Guid KnowledgeId { get; set; }
    public Guid KnowledgeDocumentId { get; set; }
}