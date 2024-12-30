using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nels.Aigc.Migrations
{
    /// <inheritdoc />
    public partial class conversationDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "DeleterId",
                table: "ai_AgentConversation",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletionTime",
                table: "ai_AgentConversation",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ai_AgentConversation",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "DeleterId",
                table: "ai_AgentChat",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletionTime",
                table: "ai_AgentChat",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ai_AgentChat",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeleterId",
                table: "ai_AgentConversation");

            migrationBuilder.DropColumn(
                name: "DeletionTime",
                table: "ai_AgentConversation");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ai_AgentConversation");

            migrationBuilder.DropColumn(
                name: "DeleterId",
                table: "ai_AgentChat");

            migrationBuilder.DropColumn(
                name: "DeletionTime",
                table: "ai_AgentChat");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ai_AgentChat");
        }
    }
}
