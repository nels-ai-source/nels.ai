using Microsoft.EntityFrameworkCore;
using Nels.Abp.SysMng;
using Nels.Aigc.Entities;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Nels.Aigc.EntityFrameworkCore;

public static class AigcDbContextModelCreatingExtensions
{

    public static void ConfigureAigc(this ModelBuilder builder)
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
        builder.Entity<Agent>(b =>
        {
            b.ToTable(AigcDbProperties.DbTablePrefix + nameof(Agent), SysMngDbProperties.DbSchema);

            b.HasMany(x => x.Questions).WithOne().HasForeignKey(x => x.AgentId);
            b.HasMany(x => x.Knowledges).WithOne().HasForeignKey(x => x.AgentId);
            b.HasMany(x => x.Tools).WithOne().HasForeignKey(x => x.AgentId);
            b.HasOne(x => x.KnowledgeOption).WithOne().HasForeignKey<AgentKnowledgeOption>(x => x.AgentId);

            b.ConfigureByConvention();
        });
        builder.Entity<AgentPresetQuestions>(b =>
        {
            b.ToTable(AigcDbProperties.DbTablePrefix + nameof(AgentPresetQuestions), SysMngDbProperties.DbSchema);

            b.ConfigureByConvention();
        });
        builder.Entity<AgentKnowledge>(b =>
        {
            b.ToTable(AigcDbProperties.DbTablePrefix + nameof(AgentKnowledge), SysMngDbProperties.DbSchema);

            b.ConfigureByConvention();
        });
        builder.Entity<AgentTool>(b =>
        {
            b.ToTable(AigcDbProperties.DbTablePrefix + nameof(AgentTool), SysMngDbProperties.DbSchema);

            b.ConfigureByConvention();
        });
        builder.Entity<AgentKnowledgeOption>(b =>
        {
            b.ToTable(AigcDbProperties.DbTablePrefix + nameof(AgentKnowledgeOption), SysMngDbProperties.DbSchema);

            b.ConfigureByConvention();
        });
        builder.Entity<Plugin>(b =>
        {
            b.ToTable(AigcDbProperties.DbTablePrefix + nameof(Plugin), SysMngDbProperties.DbSchema);

            b.ConfigureByConvention();
        });
        builder.Entity<Tool>(b =>
        {
            b.ToTable(AigcDbProperties.DbTablePrefix + nameof(Tool), SysMngDbProperties.DbSchema);
            b.HasMany(x => x.InputParamters).WithOne().HasForeignKey(x => x.ToolId);

            b.HasMany(x => x.OutputParamters).WithOne().HasForeignKey(x => x.ToolId);

            b.ConfigureByConvention();
        });
        builder.Entity<ToolParamter>(b =>
        {
            b.ToTable(AigcDbProperties.DbTablePrefix + nameof(ToolParamter), SysMngDbProperties.DbSchema);

            b.ConfigureByConvention();
        });

        builder.Entity<Conversation>(b =>
        {
            b.ToTable(AigcDbProperties.DbTablePrefix + "AgentConversation", SysMngDbProperties.DbSchema);

            b.ConfigureByConvention();
        });
        builder.Entity<Chat>(b =>
        {
            b.ToTable(AigcDbProperties.DbTablePrefix + nameof(Chat), SysMngDbProperties.DbSchema);

            b.ConfigureByConvention();
        });
        builder.Entity<ChatMessage>(b =>
        {
            b.ToTable(AigcDbProperties.DbTablePrefix + nameof(ChatMessage), SysMngDbProperties.DbSchema);

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
