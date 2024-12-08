using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nels.Aigc.Migrations
{
    /// <inheritdoc />
    public partial class add_model_Properties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Properties",
                table: "ai_Model",
                type: "character varying(512)",
                maxLength: 512,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Properties",
                table: "ai_Model");
        }
    }
}
