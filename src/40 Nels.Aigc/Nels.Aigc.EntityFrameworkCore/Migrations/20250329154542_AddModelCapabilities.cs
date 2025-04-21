using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nels.Aigc.Migrations
{
    /// <inheritdoc />
    public partial class AddModelCapabilities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Capabilities",
                table: "ai_ModelInstance",
                type: "character varying(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Connector",
                table: "ai_ModelInstance",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Capabilities",
                table: "ai_Model",
                type: "character varying(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Connector",
                table: "ai_Model",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Capabilities",
                table: "ai_ModelInstance");

            migrationBuilder.DropColumn(
                name: "Connector",
                table: "ai_ModelInstance");

            migrationBuilder.DropColumn(
                name: "Capabilities",
                table: "ai_Model");

            migrationBuilder.DropColumn(
                name: "Connector",
                table: "ai_Model");
        }
    }
}
