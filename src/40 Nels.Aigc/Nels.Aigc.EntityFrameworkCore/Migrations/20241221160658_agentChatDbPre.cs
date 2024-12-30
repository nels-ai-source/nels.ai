using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nels.Aigc.Migrations
{
    /// <inheritdoc />
    public partial class agentChatDbPre : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AgentChats_AgentConversations_AgentConversationId",
                table: "AgentChats");

            migrationBuilder.DropForeignKey(
                name: "FK_AgentMessages_AgentChats_AgentChatId",
                table: "AgentMessages");

            migrationBuilder.DropForeignKey(
                name: "FK_AgentStepLogs_AgentChats_AgentChatId",
                table: "AgentStepLogs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AgentStepLogs",
                table: "AgentStepLogs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AgentMessages",
                table: "AgentMessages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AgentConversations",
                table: "AgentConversations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AgentChats",
                table: "AgentChats");

            migrationBuilder.RenameTable(
                name: "AgentStepLogs",
                newName: "ai_AgentStepLog");

            migrationBuilder.RenameTable(
                name: "AgentMessages",
                newName: "ai_AgentMessage");

            migrationBuilder.RenameTable(
                name: "AgentConversations",
                newName: "ai_AgentConversation");

            migrationBuilder.RenameTable(
                name: "AgentChats",
                newName: "ai_AgentChat");

            migrationBuilder.RenameIndex(
                name: "IX_AgentStepLogs_AgentChatId",
                table: "ai_AgentStepLog",
                newName: "IX_ai_AgentStepLog_AgentChatId");

            migrationBuilder.RenameIndex(
                name: "IX_AgentMessages_AgentChatId",
                table: "ai_AgentMessage",
                newName: "IX_ai_AgentMessage_AgentChatId");

            migrationBuilder.RenameIndex(
                name: "IX_AgentChats_AgentConversationId",
                table: "ai_AgentChat",
                newName: "IX_ai_AgentChat_AgentConversationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ai_AgentStepLog",
                table: "ai_AgentStepLog",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ai_AgentMessage",
                table: "ai_AgentMessage",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ai_AgentConversation",
                table: "ai_AgentConversation",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ai_AgentChat",
                table: "ai_AgentChat",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ai_AgentChat_ai_AgentConversation_AgentConversationId",
                table: "ai_AgentChat",
                column: "AgentConversationId",
                principalTable: "ai_AgentConversation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ai_AgentMessage_ai_AgentChat_AgentChatId",
                table: "ai_AgentMessage",
                column: "AgentChatId",
                principalTable: "ai_AgentChat",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ai_AgentStepLog_ai_AgentChat_AgentChatId",
                table: "ai_AgentStepLog",
                column: "AgentChatId",
                principalTable: "ai_AgentChat",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ai_AgentChat_ai_AgentConversation_AgentConversationId",
                table: "ai_AgentChat");

            migrationBuilder.DropForeignKey(
                name: "FK_ai_AgentMessage_ai_AgentChat_AgentChatId",
                table: "ai_AgentMessage");

            migrationBuilder.DropForeignKey(
                name: "FK_ai_AgentStepLog_ai_AgentChat_AgentChatId",
                table: "ai_AgentStepLog");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ai_AgentStepLog",
                table: "ai_AgentStepLog");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ai_AgentMessage",
                table: "ai_AgentMessage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ai_AgentConversation",
                table: "ai_AgentConversation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ai_AgentChat",
                table: "ai_AgentChat");

            migrationBuilder.RenameTable(
                name: "ai_AgentStepLog",
                newName: "AgentStepLogs");

            migrationBuilder.RenameTable(
                name: "ai_AgentMessage",
                newName: "AgentMessages");

            migrationBuilder.RenameTable(
                name: "ai_AgentConversation",
                newName: "AgentConversations");

            migrationBuilder.RenameTable(
                name: "ai_AgentChat",
                newName: "AgentChats");

            migrationBuilder.RenameIndex(
                name: "IX_ai_AgentStepLog_AgentChatId",
                table: "AgentStepLogs",
                newName: "IX_AgentStepLogs_AgentChatId");

            migrationBuilder.RenameIndex(
                name: "IX_ai_AgentMessage_AgentChatId",
                table: "AgentMessages",
                newName: "IX_AgentMessages_AgentChatId");

            migrationBuilder.RenameIndex(
                name: "IX_ai_AgentChat_AgentConversationId",
                table: "AgentChats",
                newName: "IX_AgentChats_AgentConversationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AgentStepLogs",
                table: "AgentStepLogs",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AgentMessages",
                table: "AgentMessages",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AgentConversations",
                table: "AgentConversations",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AgentChats",
                table: "AgentChats",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AgentChats_AgentConversations_AgentConversationId",
                table: "AgentChats",
                column: "AgentConversationId",
                principalTable: "AgentConversations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AgentMessages_AgentChats_AgentChatId",
                table: "AgentMessages",
                column: "AgentChatId",
                principalTable: "AgentChats",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AgentStepLogs_AgentChats_AgentChatId",
                table: "AgentStepLogs",
                column: "AgentChatId",
                principalTable: "AgentChats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
