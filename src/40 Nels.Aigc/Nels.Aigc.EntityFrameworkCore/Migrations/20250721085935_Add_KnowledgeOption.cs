using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nels.Aigc.Migrations
{
    /// <inheritdoc />
    public partial class Add_KnowledgeOption : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AgentKnowledgeOption",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AgentId = table.Column<Guid>(type: "uuid", nullable: false),
                    AutoInvoke = table.Column<bool>(type: "boolean", nullable: false),
                    SearchStrategy = table.Column<int>(type: "integer", nullable: false),
                    MaxRecallCount = table.Column<int>(type: "integer", nullable: false),
                    MinMatchScore = table.Column<double>(type: "double precision", nullable: false),
                    ReplyMode = table.Column<int>(type: "integer", nullable: false),
                    CustomReply = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    ShowSource = table.Column<bool>(type: "boolean", nullable: false),
                    SourceDisplayMode = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AgentKnowledgeOption", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AgentKnowledgeOption_ai_Agent_AgentId",
                        column: x => x.AgentId,
                        principalTable: "ai_Agent",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AgentKnowledgeOption_AgentId",
                table: "AgentKnowledgeOption",
                column: "AgentId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AgentKnowledgeOption");
        }
    }
}
