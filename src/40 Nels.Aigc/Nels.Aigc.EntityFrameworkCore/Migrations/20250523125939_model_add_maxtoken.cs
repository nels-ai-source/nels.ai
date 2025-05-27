using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nels.Aigc.Migrations
{
    /// <inheritdoc />
    public partial class model_add_maxtoken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ai_AgentChat_ai_AgentConversation_AgentConversationId",
                table: "ai_AgentChat");

            migrationBuilder.DropTable(
                name: "ai_ModelInstance");

            migrationBuilder.DropIndex(
                name: "IX_ai_AgentChat_AgentConversationId",
                table: "ai_AgentChat");

            migrationBuilder.DropColumn(
                name: "Metadata",
                table: "ai_Model");

            migrationBuilder.DropColumn(
                name: "Properties",
                table: "ai_Model");

            migrationBuilder.AddColumn<int>(
                name: "MaxTokens",
                table: "ai_Model",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "AgentConversationEntityId",
                table: "ai_AgentChat",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ai_AgentChat_AgentConversationEntityId",
                table: "ai_AgentChat",
                column: "AgentConversationEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_ai_AgentChat_ai_AgentConversation_AgentConversationEntityId",
                table: "ai_AgentChat",
                column: "AgentConversationEntityId",
                principalTable: "ai_AgentConversation",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ai_AgentChat_ai_AgentConversation_AgentConversationEntityId",
                table: "ai_AgentChat");

            migrationBuilder.DropIndex(
                name: "IX_ai_AgentChat_AgentConversationEntityId",
                table: "ai_AgentChat");

            migrationBuilder.DropColumn(
                name: "MaxTokens",
                table: "ai_Model");

            migrationBuilder.DropColumn(
                name: "AgentConversationEntityId",
                table: "ai_AgentChat");

            migrationBuilder.AddColumn<string>(
                name: "Metadata",
                table: "ai_Model",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Properties",
                table: "ai_Model",
                type: "character varying(512)",
                maxLength: 512,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "ai_ModelInstance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AccessKey = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Capabilities = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Connector = table.Column<int>(type: "integer", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: true),
                    DeploymentName = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Description = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false),
                    Endpoint = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    IsDefault = table.Column<bool>(type: "boolean", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uuid", nullable: true),
                    ModelId = table.Column<Guid>(type: "uuid", nullable: true),
                    Name = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Provider = table.Column<int>(type: "integer", nullable: false),
                    SecretKey = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ai_ModelInstance", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ai_AgentChat_AgentConversationId",
                table: "ai_AgentChat",
                column: "AgentConversationId");

            migrationBuilder.AddForeignKey(
                name: "FK_ai_AgentChat_ai_AgentConversation_AgentConversationId",
                table: "ai_AgentChat",
                column: "AgentConversationId",
                principalTable: "ai_AgentConversation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
