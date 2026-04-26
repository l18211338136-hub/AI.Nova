using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AI.Nova.Server.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddDocumentContentToKnowledgeDocument : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "Content",
                table: "KnowledgeDocuments",
                type: "bytea",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContentType",
                table: "KnowledgeDocuments",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Content",
                table: "KnowledgeDocuments");

            migrationBuilder.DropColumn(
                name: "ContentType",
                table: "KnowledgeDocuments");
        }
    }
}
