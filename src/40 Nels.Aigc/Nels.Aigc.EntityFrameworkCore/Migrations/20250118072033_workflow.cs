using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nels.Aigc.Migrations
{
    /// <inheritdoc />
    public partial class workflow : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ai_AgentPresetQuestions_ai_Agent_AgentId",
                table: "ai_AgentPresetQuestions");

            migrationBuilder.DropTable(
                name: "ai_AgentMetadata");

            migrationBuilder.DropIndex(
                name: "IX_ai_AgentPresetQuestions_AgentId",
                table: "ai_AgentPresetQuestions");

            migrationBuilder.AddColumn<Guid>(
                name: "AgentEntityId",
                table: "ai_AgentPresetQuestions",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "MetadataId",
                table: "ai_Agent",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ai_WorkflowAgentMetadata",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AgentId = table.Column<Guid>(type: "uuid", nullable: false),
                    Steps = table.Column<string>(type: "text", nullable: false),
                    States = table.Column<string>(type: "text", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ai_WorkflowAgentMetadata", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ai_AgentPresetQuestions_AgentEntityId",
                table: "ai_AgentPresetQuestions",
                column: "AgentEntityId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_ai_AgentPresetQuestions_ai_Agent_AgentEntityId",
                table: "ai_AgentPresetQuestions",
                column: "AgentEntityId",
                principalTable: "ai_Agent",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ai_Agent_ai_WorkflowAgentMetadata_MetadataId",
                table: "ai_Agent");

            migrationBuilder.DropForeignKey(
                name: "FK_ai_AgentPresetQuestions_ai_Agent_AgentEntityId",
                table: "ai_AgentPresetQuestions");

            migrationBuilder.DropTable(
                name: "ai_WorkflowAgentMetadata");

            migrationBuilder.DropIndex(
                name: "IX_ai_AgentPresetQuestions_AgentEntityId",
                table: "ai_AgentPresetQuestions");

            migrationBuilder.DropIndex(
                name: "IX_ai_Agent_MetadataId",
                table: "ai_Agent");

            migrationBuilder.DropColumn(
                name: "AgentEntityId",
                table: "ai_AgentPresetQuestions");

            migrationBuilder.DropColumn(
                name: "MetadataId",
                table: "ai_Agent");

            migrationBuilder.CreateTable(
                name: "ai_AgentMetadata",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AgentId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uuid", nullable: true),
                    States = table.Column<string>(type: "text", nullable: false),
                    Steps = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ai_AgentMetadata", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ai_AgentMetadata_ai_Agent_AgentId",
                        column: x => x.AgentId,
                        principalTable: "ai_Agent",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ai_AgentPresetQuestions_AgentId",
                table: "ai_AgentPresetQuestions",
                column: "AgentId");

            migrationBuilder.CreateIndex(
                name: "IX_ai_AgentMetadata_AgentId",
                table: "ai_AgentMetadata",
                column: "AgentId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ai_AgentPresetQuestions_ai_Agent_AgentId",
                table: "ai_AgentPresetQuestions",
                column: "AgentId",
                principalTable: "ai_Agent",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
