using System;

namespace Nels.Aigc.Knowledges;

public class DocumentSplitArgs
{
    public Guid KnowledgeDocumentId { get; set; }
    public Guid FileId { get; set; }
    public int MaxTokensPerParagraph { get; set; } = 500;
    public int OverlapTokens { get; set; } = 0;
}