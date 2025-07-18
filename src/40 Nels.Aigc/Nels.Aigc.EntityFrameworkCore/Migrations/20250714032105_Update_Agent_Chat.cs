using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nels.Aigc.Migrations
{
    /// <inheritdoc />
    public partial class Update_Agent_Chat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ai_AgentChat_ai_AgentConversation_AgentConversationEntityId",
                table: "ai_AgentChat");

            migrationBuilder.DropTable(
                name: "ai_AgentStepLog");

            migrationBuilder.DropTable(
                name: "ai_LlmAgentMetadata");

            migrationBuilder.DropTable(
                name: "ai_WorkflowAgentMetadata");

            migrationBuilder.DropIndex(
                name: "IX_ai_AgentChat_AgentConversationEntityId",
                table: "ai_AgentChat");

            migrationBuilder.DropColumn(
                name: "CreationTime",
                table: "sys_Roles");

            migrationBuilder.DropColumn(
                name: "CreationTime",
                table: "sys_ClaimTypes");

            migrationBuilder.DropColumn(
                name: "ApplicationName",
                table: "sys_BackgroundJobs");

            migrationBuilder.DropColumn(
                name: "ContentType",
                table: "ai_AgentMessage");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "ai_AgentMessage");

            migrationBuilder.DropColumn(
                name: "AgentConversationEntityId",
                table: "ai_AgentChat");

            migrationBuilder.RenameColumn(
                name: "AgentConversationId",
                table: "ai_AgentChat",
                newName: "SpaceId");

            migrationBuilder.AddColumn<Guid>(
                name: "SpaceId",
                table: "ai_AgentMessage",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SpaceId",
                table: "ai_AgentConversation",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ConversationId",
                table: "ai_AgentChat",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<double>(
                name: "FirstTokenResponseDuration",
                table: "ai_AgentChat",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "InputTokenCount",
                table: "ai_AgentChat",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OutputTokenCount",
                table: "ai_AgentChat",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "TotalResponseDuration",
                table: "ai_AgentChat",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "TotalTokenCount",
                table: "ai_AgentChat",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ai_AgentChat_ConversationId",
                table: "ai_AgentChat",
                column: "ConversationId");

            migrationBuilder.AddForeignKey(
                name: "FK_ai_AgentChat_ai_AgentConversation_ConversationId",
                table: "ai_AgentChat",
                column: "ConversationId",
                principalTable: "ai_AgentConversation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ai_AgentChat_ai_AgentConversation_ConversationId",
                table: "ai_AgentChat");

            migrationBuilder.DropIndex(
                name: "IX_ai_AgentChat_ConversationId",
                table: "ai_AgentChat");

            migrationBuilder.DropColumn(
                name: "SpaceId",
                table: "ai_AgentMessage");

            migrationBuilder.DropColumn(
                name: "SpaceId",
                table: "ai_AgentConversation");

            migrationBuilder.DropColumn(
                name: "ConversationId",
                table: "ai_AgentChat");

            migrationBuilder.DropColumn(
                name: "FirstTokenResponseDuration",
                table: "ai_AgentChat");

            migrationBuilder.DropColumn(
                name: "InputTokenCount",
                table: "ai_AgentChat");

            migrationBuilder.DropColumn(
                name: "OutputTokenCount",
                table: "ai_AgentChat");

            migrationBuilder.DropColumn(
                name: "TotalResponseDuration",
                table: "ai_AgentChat");

            migrationBuilder.DropColumn(
                name: "TotalTokenCount",
                table: "ai_AgentChat");

            migrationBuilder.RenameColumn(
                name: "SpaceId",
                table: "ai_AgentChat",
                newName: "AgentConversationId");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationTime",
                table: "sys_Roles",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationTime",
                table: "sys_ClaimTypes",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ApplicationName",
                table: "sys_BackgroundJobs",
                type: "character varying(96)",
                maxLength: 96,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContentType",
                table: "ai_AgentMessage",
                type: "character varying(32)",
                maxLength: 32,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "ai_AgentMessage",
                type: "character varying(32)",
                maxLength: 32,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "AgentConversationEntityId",
                table: "ai_AgentChat",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ai_AgentStepLog",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AgentChatId = table.Column<Guid>(type: "uuid", nullable: false),
                    AgentConversationId = table.Column<Guid>(type: "uuid", nullable: false),
                    AgentId = table.Column<Guid>(type: "uuid", nullable: false),
                    CompleteTokens = table.Column<int>(type: "integer", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: true),
                    DeleterId = table.Column<Guid>(type: "uuid", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Duration = table.Column<double>(type: "double precision", nullable: false),
                    Index = table.Column<int>(type: "integer", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    LastModificationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uuid", nullable: true),
                    ModelId = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    PromptTokens = table.Column<int>(type: "integer", nullable: false),
                    StepId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ai_AgentStepLog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ai_AgentStepLog_ai_AgentChat_AgentChatId",
                        column: x => x.AgentChatId,
                        principalTable: "ai_AgentChat",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ai_LlmAgentMetadata",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AgentId = table.Column<Guid>(type: "uuid", nullable: false),
                    ChatReducerCount = table.Column<int>(type: "integer", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uuid", nullable: true),
                    Prompt = table.Column<string>(type: "text", nullable: false),
                    ToolAutoInvoke = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ai_LlmAgentMetadata", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ai_WorkflowAgentMetadata",
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
                    table.PrimaryKey("PK_ai_WorkflowAgentMetadata", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ai_AgentChat_AgentConversationEntityId",
                table: "ai_AgentChat",
                column: "AgentConversationEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_ai_AgentStepLog_AgentChatId",
                table: "ai_AgentStepLog",
                column: "AgentChatId");

            migrationBuilder.AddForeignKey(
                name: "FK_ai_AgentChat_ai_AgentConversation_AgentConversationEntityId",
                table: "ai_AgentChat",
                column: "AgentConversationEntityId",
                principalTable: "ai_AgentConversation",
                principalColumn: "Id");
        }
    }
}
