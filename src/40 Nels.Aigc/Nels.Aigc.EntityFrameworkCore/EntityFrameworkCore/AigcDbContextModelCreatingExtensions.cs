using Microsoft.EntityFrameworkCore;
using Nels.Abp.SysMng;
using Nels.Aigc.Entities;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Nels.Aigc.EntityFrameworkCore;

public static class AigcDbContextModelCreatingExtensions
{

    public static void ConfigureAigc(
        this ModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));

        #region aigc
        builder.Entity<Model>(b =>
        {
            b.ToTable(AigcDbProperties.DbTablePrefix + nameof(Model), AigcDbProperties.DbSchema);

            b.ConfigureByConvention();
        });
        builder.Entity<Prompt>(b =>
        {
            b.ToTable(AigcDbProperties.DbTablePrefix + nameof(Prompt), AigcDbProperties.DbSchema);

            b.ConfigureByConvention();
        });
        builder.Entity<AgentEntity>(b =>
        {
            b.ToTable(AigcDbProperties.DbTablePrefix + "Agent", SysMngDbProperties.DbSchema);

            b.ConfigureByConvention();
        });
        builder.Entity<AgentPresetQuestions>(b =>
        {
            b.ToTable(AigcDbProperties.DbTablePrefix + nameof(AgentPresetQuestions), SysMngDbProperties.DbSchema);

            b.ConfigureByConvention();
        });
        builder.Entity<WorkflowAgentMetadata>(b =>
        {
            b.ToTable(AigcDbProperties.DbTablePrefix + nameof(WorkflowAgentMetadata), SysMngDbProperties.DbSchema);

            b.ConfigureByConvention();
        });
        builder.Entity<LlmAgentMetadata>(b =>
        {
            b.ToTable(AigcDbProperties.DbTablePrefix + nameof(LlmAgentMetadata), SysMngDbProperties.DbSchema);

            b.ConfigureByConvention();
        });
        builder.Entity<AgentConversationEntity>(b =>
        {
            b.ToTable(AigcDbProperties.DbTablePrefix + "AgentConversation", SysMngDbProperties.DbSchema);

            b.ConfigureByConvention();
        });
        builder.Entity<AgentChat>(b =>
        {
            b.ToTable(AigcDbProperties.DbTablePrefix + nameof(AgentChat), SysMngDbProperties.DbSchema);

            b.ConfigureByConvention();
        });
        builder.Entity<AgentMessage>(b =>
        {
            b.ToTable(AigcDbProperties.DbTablePrefix + nameof(AgentMessage), SysMngDbProperties.DbSchema);

            b.ConfigureByConvention();
        });
        builder.Entity<AgentStepLog>(b =>
        {
            b.ToTable(AigcDbProperties.DbTablePrefix + nameof(AgentStepLog), SysMngDbProperties.DbSchema);

            b.ConfigureByConvention();
        });
        builder.Entity<Space>(b =>
        {
            b.ToTable(AigcDbProperties.DbTablePrefix + nameof(Space), SysMngDbProperties.DbSchema);

            b.ConfigureByConvention();
        });
        builder.Entity<SpaceUser>(b =>
        {
            b.ToTable(AigcDbProperties.DbTablePrefix + nameof(SpaceUser), SysMngDbProperties.DbSchema);

            b.ConfigureByConvention();
        });

        builder.Entity<Knowledge>(b =>
        {
            b.ToTable(AigcDbProperties.DbTablePrefix + nameof(Knowledge), SysMngDbProperties.DbSchema);

            b.ConfigureByConvention();
        });
        builder.Entity<KnowledgeDocument>(b =>
        {
            b.ToTable(AigcDbProperties.DbTablePrefix + nameof(KnowledgeDocument), SysMngDbProperties.DbSchema);

            b.ConfigureByConvention();
        });
        builder.Entity<KnowledgeDocumentParagraph>(b =>
        {
            b.ToTable(AigcDbProperties.DbTablePrefix + nameof(KnowledgeDocumentParagraph), SysMngDbProperties.DbSchema);

            b.ConfigureByConvention();
        });
        #endregion

        if (builder.IsTenantOnlyDatabase())
        {
            return;
        }
    }
}
