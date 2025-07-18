using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nels.Aigc.Migrations
{
    /// <inheritdoc />
    public partial class UpdateChatMessage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ai_AgentMessage");

            migrationBuilder.DropTable(
                name: "ai_AgentChat");

            migrationBuilder.DropColumn(
                name: "AgentId",
                table: "ai_AgentConversation");

            migrationBuilder.CreateTable(
                name: "ai_Chat",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SpaceId = table.Column<Guid>(type: "uuid", nullable: false),
                    ConversationId = table.Column<Guid>(type: "uuid", nullable: false),
                    Question = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Answer = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: false),
                    TotalResponseDuration = table.Column<double>(type: "double precision", nullable: false),
                    FirstTokenResponseDuration = table.Column<double>(type: "double precision", nullable: false),
                    InputTokenCount = table.Column<int>(type: "integer", nullable: false),
                    OutputTokenCount = table.Column<int>(type: "integer", nullable: false),
                    TotalTokenCount = table.Column<int>(type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_ai_Chat", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ai_Chat_ai_AgentConversation_ConversationId",
                        column: x => x.ConversationId,
                        principalTable: "ai_AgentConversation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ai_ChatMessage",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SpaceId = table.Column<Guid>(type: "uuid", nullable: false),
                    ConversationId = table.Column<Guid>(type: "uuid", nullable: false),
                    ChatId = table.Column<Guid>(type: "uuid", nullable: false),
                    Role = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false),
                    Metadata = table.Column<string>(type: "text", nullable: false),
                    Index = table.Column<int>(type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_ai_ChatMessage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ai_ChatMessage_ai_Chat_ChatId",
                        column: x => x.ChatId,
                        principalTable: "ai_Chat",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ai_Chat_ConversationId",
                table: "ai_Chat",
                column: "ConversationId");

            migrationBuilder.CreateIndex(
                name: "IX_ai_ChatMessage_ChatId",
                table: "ai_ChatMessage",
                column: "ChatId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ai_ChatMessage");

            migrationBuilder.DropTable(
                name: "ai_Chat");

            migrationBuilder.AddColumn<Guid>(
                name: "AgentId",
                table: "ai_AgentConversation",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "ai_AgentChat",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AgentId = table.Column<Guid>(type: "uuid", nullable: false),
                    Answer = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: false),
                    ConversationId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: true),
                    DeleterId = table.Column<Guid>(type: "uuid", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    FirstTokenResponseDuration = table.Column<double>(type: "double precision", nullable: false),
                    InputTokenCount = table.Column<int>(type: "integer", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    LastModificationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uuid", nullable: true),
                    OutputTokenCount = table.Column<int>(type: "integer", nullable: false),
                    Question = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    SpaceId = table.Column<Guid>(type: "uuid", nullable: false),
                    TotalResponseDuration = table.Column<double>(type: "double precision", nullable: false),
                    TotalTokenCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ai_AgentChat", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ai_AgentChat_ai_AgentConversation_ConversationId",
                        column: x => x.ConversationId,
                        principalTable: "ai_AgentConversation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ai_AgentMessage",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AgentChatId = table.Column<Guid>(type: "uuid", nullable: false),
                    AgentConversationId = table.Column<Guid>(type: "uuid", nullable: false),
                    AgentId = table.Column<Guid>(type: "uuid", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: true),
                    DeleterId = table.Column<Guid>(type: "uuid", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Index = table.Column<int>(type: "integer", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    LastModificationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uuid", nullable: true),
                    Metadata = table.Column<string>(type: "text", nullable: false),
                    Role = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    SpaceId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ai_AgentMessage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ai_AgentMessage_ai_AgentChat_AgentChatId",
                        column: x => x.AgentChatId,
                        principalTable: "ai_AgentChat",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ai_AgentChat_ConversationId",
                table: "ai_AgentChat",
                column: "ConversationId");

            migrationBuilder.CreateIndex(
                name: "IX_ai_AgentMessage_AgentChatId",
                table: "ai_AgentMessage",
                column: "AgentChatId");
        }
    }
}
