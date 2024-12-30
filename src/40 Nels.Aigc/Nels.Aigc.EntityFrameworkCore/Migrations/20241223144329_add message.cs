using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nels.Aigc.Migrations
{
    /// <inheritdoc />
    public partial class addmessage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ai_AgentMessage_ai_AgentChat_AgentChatId",
                table: "ai_AgentMessage");

            migrationBuilder.AddColumn<Guid>(
                name: "DeleterId",
                table: "ai_AgentStepLog",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletionTime",
                table: "ai_AgentStepLog",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ai_AgentStepLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ModelId",
                table: "ai_AgentStepLog",
                type: "character varying(32)",
                maxLength: 32,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<Guid>(
                name: "AgentChatId",
                table: "ai_AgentMessage",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeleterId",
                table: "ai_AgentMessage",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletionTime",
                table: "ai_AgentMessage",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ai_AgentMessage",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_ai_AgentMessage_ai_AgentChat_AgentChatId",
                table: "ai_AgentMessage",
                column: "AgentChatId",
                principalTable: "ai_AgentChat",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ai_AgentMessage_ai_AgentChat_AgentChatId",
                table: "ai_AgentMessage");

            migrationBuilder.DropColumn(
                name: "DeleterId",
                table: "ai_AgentStepLog");

            migrationBuilder.DropColumn(
                name: "DeletionTime",
                table: "ai_AgentStepLog");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ai_AgentStepLog");

            migrationBuilder.DropColumn(
                name: "ModelId",
                table: "ai_AgentStepLog");

            migrationBuilder.DropColumn(
                name: "DeleterId",
                table: "ai_AgentMessage");

            migrationBuilder.DropColumn(
                name: "DeletionTime",
                table: "ai_AgentMessage");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ai_AgentMessage");

            migrationBuilder.AlterColumn<Guid>(
                name: "AgentChatId",
                table: "ai_AgentMessage",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_ai_AgentMessage_ai_AgentChat_AgentChatId",
                table: "ai_AgentMessage",
                column: "AgentChatId",
                principalTable: "ai_AgentChat",
                principalColumn: "Id");
        }
    }
}
