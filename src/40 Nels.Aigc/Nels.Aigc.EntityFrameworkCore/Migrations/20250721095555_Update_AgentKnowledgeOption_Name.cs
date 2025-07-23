using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nels.Aigc.Migrations
{
    /// <inheritdoc />
    public partial class Update_AgentKnowledgeOption_Name : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AgentKnowledgeOption_ai_Agent_AgentId",
                table: "AgentKnowledgeOption");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AgentKnowledgeOption",
                table: "AgentKnowledgeOption");

            migrationBuilder.RenameTable(
                name: "AgentKnowledgeOption",
                newName: "ai_AgentKnowledgeOption");

            migrationBuilder.RenameIndex(
                name: "IX_AgentKnowledgeOption_AgentId",
                table: "ai_AgentKnowledgeOption",
                newName: "IX_ai_AgentKnowledgeOption_AgentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ai_AgentKnowledgeOption",
                table: "ai_AgentKnowledgeOption",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ai_AgentKnowledgeOption_ai_Agent_AgentId",
                table: "ai_AgentKnowledgeOption",
                column: "AgentId",
                principalTable: "ai_Agent",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ai_AgentKnowledgeOption_ai_Agent_AgentId",
                table: "ai_AgentKnowledgeOption");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ai_AgentKnowledgeOption",
                table: "ai_AgentKnowledgeOption");

            migrationBuilder.RenameTable(
                name: "ai_AgentKnowledgeOption",
                newName: "AgentKnowledgeOption");

            migrationBuilder.RenameIndex(
                name: "IX_ai_AgentKnowledgeOption_AgentId",
                table: "AgentKnowledgeOption",
                newName: "IX_AgentKnowledgeOption_AgentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AgentKnowledgeOption",
                table: "AgentKnowledgeOption",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AgentKnowledgeOption_ai_Agent_AgentId",
                table: "AgentKnowledgeOption",
                column: "AgentId",
                principalTable: "ai_Agent",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
