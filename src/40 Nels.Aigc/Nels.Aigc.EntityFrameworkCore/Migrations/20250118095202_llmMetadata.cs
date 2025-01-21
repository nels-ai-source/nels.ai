using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nels.Aigc.Migrations
{
    /// <inheritdoc />
    public partial class llmMetadata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ai_Agent_ai_WorkflowAgentMetadata_MetadataId",
                table: "ai_Agent");

            migrationBuilder.DropIndex(
                name: "IX_ai_Agent_MetadataId",
                table: "ai_Agent");

            migrationBuilder.DropColumn(
                name: "MetadataId",
                table: "ai_Agent");

            migrationBuilder.CreateTable(
                name: "ai_LlmAgentMetadata",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AgentId = table.Column<Guid>(type: "uuid", nullable: false),
                    Prompt = table.Column<string>(type: "text", nullable: false),
                    ChatReducerCount = table.Column<int>(type: "integer", nullable: false),
                    ToolAutoInvoke = table.Column<bool>(type: "boolean", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ai_LlmAgentMetadata", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ai_LlmAgentMetadata");

            migrationBuilder.AddColumn<Guid>(
                name: "MetadataId",
                table: "ai_Agent",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ai_Agent_MetadataId",
                table: "ai_Agent",
                column: "MetadataId");

            migrationBuilder.AddForeignKey(
                name: "FK_ai_Agent_ai_WorkflowAgentMetadata_MetadataId",
                table: "ai_Agent",
                column: "MetadataId",
                principalTable: "ai_WorkflowAgentMetadata",
                principalColumn: "Id");
        }
    }
}
