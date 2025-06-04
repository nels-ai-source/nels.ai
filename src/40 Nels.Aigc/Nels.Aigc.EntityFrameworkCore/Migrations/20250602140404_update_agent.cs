using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nels.Aigc.Migrations
{
    /// <inheritdoc />
    public partial class update_agent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ai_AgentPresetQuestions_ai_Agent_AgentEntityId",
                table: "ai_AgentPresetQuestions");

            migrationBuilder.DropIndex(
                name: "IX_ai_AgentPresetQuestions_AgentEntityId",
                table: "ai_AgentPresetQuestions");

            migrationBuilder.DropColumn(
                name: "AgentEntityId",
                table: "ai_AgentPresetQuestions");

            migrationBuilder.DropColumn(
                name: "CreationTime",
                table: "ai_AgentPresetQuestions");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "ai_AgentPresetQuestions");

            migrationBuilder.DropColumn(
                name: "LastModificationTime",
                table: "ai_AgentPresetQuestions");

            migrationBuilder.DropColumn(
                name: "LastModifierId",
                table: "ai_AgentPresetQuestions");

            migrationBuilder.RenameColumn(
                name: "IntroductionText",
                table: "ai_Agent",
                newName: "Instructions");

            migrationBuilder.RenameColumn(
                name: "AgentType",
                table: "ai_Agent",
                newName: "Type");

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "ai_AgentPresetQuestions",
                type: "character varying(256)",
                maxLength: 256,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "Icon",
                table: "ai_Agent",
                type: "character varying(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Prologue",
                table: "ai_Agent",
                type: "character varying(512)",
                maxLength: 512,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "ai_AgentKnowledge",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AgentId = table.Column<Guid>(type: "uuid", nullable: false),
                    KnowledgeId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ai_AgentKnowledge", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ai_AgentKnowledge_ai_Agent_AgentId",
                        column: x => x.AgentId,
                        principalTable: "ai_Agent",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ai_AgentTool",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AgentId = table.Column<Guid>(type: "uuid", nullable: false),
                    ToolId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ai_AgentTool", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ai_AgentTool_ai_Agent_AgentId",
                        column: x => x.AgentId,
                        principalTable: "ai_Agent",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ai_Plugin",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SpaceId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Description = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Version = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    ManifestUrl = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Icon = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(type: "uuid", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ai_Plugin", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ai_Tool",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PluginId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Description = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ai_Tool", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ai_Tool_ai_Plugin_PluginId",
                        column: x => x.PluginId,
                        principalTable: "ai_Plugin",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ai_ToolParamter",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ToolId = table.Column<Guid>(type: "uuid", nullable: false),
                    ParameterDirection = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Description = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Type = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    Required = table.Column<bool>(type: "boolean", nullable: false),
                    ToolId1 = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ai_ToolParamter", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ai_ToolParamter_ai_Tool_ToolId",
                        column: x => x.ToolId,
                        principalTable: "ai_Tool",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ai_ToolParamter_ai_Tool_ToolId1",
                        column: x => x.ToolId1,
                        principalTable: "ai_Tool",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ai_AgentPresetQuestions_AgentId",
                table: "ai_AgentPresetQuestions",
                column: "AgentId");

            migrationBuilder.CreateIndex(
                name: "IX_ai_AgentKnowledge_AgentId",
                table: "ai_AgentKnowledge",
                column: "AgentId");

            migrationBuilder.CreateIndex(
                name: "IX_ai_AgentTool_AgentId",
                table: "ai_AgentTool",
                column: "AgentId");

            migrationBuilder.CreateIndex(
                name: "IX_ai_Tool_PluginId",
                table: "ai_Tool",
                column: "PluginId");

            migrationBuilder.CreateIndex(
                name: "IX_ai_ToolParamter_ToolId",
                table: "ai_ToolParamter",
                column: "ToolId");

            migrationBuilder.CreateIndex(
                name: "IX_ai_ToolParamter_ToolId1",
                table: "ai_ToolParamter",
                column: "ToolId1");

            migrationBuilder.AddForeignKey(
                name: "FK_ai_AgentPresetQuestions_ai_Agent_AgentId",
                table: "ai_AgentPresetQuestions",
                column: "AgentId",
                principalTable: "ai_Agent",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ai_AgentPresetQuestions_ai_Agent_AgentId",
                table: "ai_AgentPresetQuestions");

            migrationBuilder.DropTable(
                name: "ai_AgentKnowledge");

            migrationBuilder.DropTable(
                name: "ai_AgentTool");

            migrationBuilder.DropTable(
                name: "ai_ToolParamter");

            migrationBuilder.DropTable(
                name: "ai_Tool");

            migrationBuilder.DropTable(
                name: "ai_Plugin");

            migrationBuilder.DropIndex(
                name: "IX_ai_AgentPresetQuestions_AgentId",
                table: "ai_AgentPresetQuestions");

            migrationBuilder.DropColumn(
                name: "Icon",
                table: "ai_Agent");

            migrationBuilder.DropColumn(
                name: "Prologue",
                table: "ai_Agent");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "ai_Agent",
                newName: "AgentType");

            migrationBuilder.RenameColumn(
                name: "Instructions",
                table: "ai_Agent",
                newName: "IntroductionText");

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "ai_AgentPresetQuestions",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(256)",
                oldMaxLength: 256);

            migrationBuilder.AddColumn<Guid>(
                name: "AgentEntityId",
                table: "ai_AgentPresetQuestions",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationTime",
                table: "ai_AgentPresetQuestions",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "CreatorId",
                table: "ai_AgentPresetQuestions",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModificationTime",
                table: "ai_AgentPresetQuestions",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "LastModifierId",
                table: "ai_AgentPresetQuestions",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ai_AgentPresetQuestions_AgentEntityId",
                table: "ai_AgentPresetQuestions",
                column: "AgentEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_ai_AgentPresetQuestions_ai_Agent_AgentEntityId",
                table: "ai_AgentPresetQuestions",
                column: "AgentEntityId",
                principalTable: "ai_Agent",
                principalColumn: "Id");
        }
    }
}
