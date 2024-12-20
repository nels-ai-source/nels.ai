using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nels.Aigc.Migrations
{
    /// <inheritdoc />
    public partial class add_agent_steps : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Metadata",
                table: "ai_AgentMetadata",
                newName: "Steps");

            migrationBuilder.AddColumn<string>(
                name: "States",
                table: "ai_AgentMetadata",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "States",
                table: "ai_AgentMetadata");

            migrationBuilder.RenameColumn(
                name: "Steps",
                table: "ai_AgentMetadata",
                newName: "Metadata");
        }
    }
}
