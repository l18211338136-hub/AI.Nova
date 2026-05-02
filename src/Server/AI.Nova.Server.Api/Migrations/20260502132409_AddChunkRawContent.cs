using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AI.Nova.Server.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddChunkRawContent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RawContent",
                table: "KnowledgeDocumentChunks",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RawContent",
                table: "KnowledgeDocumentChunks");
        }
    }
}
