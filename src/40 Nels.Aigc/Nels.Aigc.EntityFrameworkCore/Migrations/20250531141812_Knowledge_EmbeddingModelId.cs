using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nels.Aigc.Migrations
{
    /// <inheritdoc />
    public partial class Knowledge_EmbeddingModelId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ModelId",
                table: "ai_Knowledge",
                newName: "EmbeddingModelId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EmbeddingModelId",
                table: "ai_Knowledge",
                newName: "ModelId");
        }
    }
}
