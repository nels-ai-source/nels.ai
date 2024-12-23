using Microsoft.EntityFrameworkCore;
using Nels.Abp.SysMng.EntityFrameworkCore;
using Nels.Aigc.Entities;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Nels.Aigc.EntityFrameworkCore;

[ConnectionStringName(AigcDbProperties.ConnectionStringName)]
public interface IAigcDbContext : ISysMngDbContext, IEfCoreDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * DbSet<Question> Questions { get; }
     */
    public DbSet<Model> Models { get; set; }
    public DbSet<ModelInstance> ModelInstances { get; set; }
    public DbSet<Prompt> Prompts { get; set; }
    public DbSet<Agent> Agents { get; set; }
    public DbSet<AgentPresetQuestions> AgentPresetQuestions { get; set; }
    public DbSet<AgentMetadata> AgentMetadatas { get; set; }
    public DbSet<AgentConversation> AgentConversations { get; set; }
    public DbSet<AgentChat> AgentChats { get; set; }
    public DbSet<AgentMessage> AgentMessages { get; set; }
    public DbSet<AgentStepLog> AgentStepLogs { get; set; }
    public DbSet<Space> Spaces { get; set; }
    public DbSet<SpaceUser> SpaceUsers { get; set; }

    public DbSet<Knowledge> Knowledges { get; set; }
    public DbSet<KnowledgeDocument> KnowledgeDocuments { get; set; }
    public DbSet<KnowledgeDocumentParagraph> KnowledgeDocumentParagraphs { get; set; }
}
