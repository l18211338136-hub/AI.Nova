using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AI.Nova.Server.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddIndexToKnowledgeDocumentChunks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Index",
                table: "KnowledgeDocumentChunks",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Index",
                table: "KnowledgeDocumentChunks");
        }
    }
}
