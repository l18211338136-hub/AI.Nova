using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Pgvector;

#nullable disable

namespace AI.Nova.Server.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddKnowledgeEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "KnowledgeBases",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuidv7()"),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "记录创建时间"),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true, comment: "记录创建者ID"),
                    ModifiedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "记录最后修改时间"),
                    ModifiedBy = table.Column<Guid>(type: "uuid", nullable: true, comment: "记录最后修改者ID"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: true, comment: "软删除标记：true表示已删除"),
                    DeletedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "记录删除时间"),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true, comment: "记录删除者ID")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KnowledgeBases", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KnowledgeDocuments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuidv7()"),
                    Title = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false),
                    KnowledgeBaseId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "记录创建时间"),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true, comment: "记录创建者ID"),
                    ModifiedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "记录最后修改时间"),
                    ModifiedBy = table.Column<Guid>(type: "uuid", nullable: true, comment: "记录最后修改者ID"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: true, comment: "软删除标记：true表示已删除"),
                    DeletedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "记录删除时间"),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true, comment: "记录删除者ID")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KnowledgeDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KnowledgeDocuments_KnowledgeBases_KnowledgeBaseId",
                        column: x => x.KnowledgeBaseId,
                        principalTable: "KnowledgeBases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KnowledgeDocumentChunks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuidv7()"),
                    Content = table.Column<string>(type: "text", nullable: false),
                    TokenCount = table.Column<int>(type: "integer", nullable: true),
                    DocumentId = table.Column<Guid>(type: "uuid", nullable: true),
                    Embedding = table.Column<Vector>(type: "vector(768)", nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "记录创建时间"),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true, comment: "记录创建者ID"),
                    ModifiedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "记录最后修改时间"),
                    ModifiedBy = table.Column<Guid>(type: "uuid", nullable: true, comment: "记录最后修改者ID"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: true, comment: "软删除标记：true表示已删除"),
                    DeletedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "记录删除时间"),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true, comment: "记录删除者ID")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KnowledgeDocumentChunks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KnowledgeDocumentChunks_KnowledgeDocuments_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "KnowledgeDocuments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_KnowledgeDocumentChunks_DocumentId",
                table: "KnowledgeDocumentChunks",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_KnowledgeDocuments_KnowledgeBaseId",
                table: "KnowledgeDocuments",
                column: "KnowledgeBaseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KnowledgeDocumentChunks");

            migrationBuilder.DropTable(
                name: "KnowledgeDocuments");

            migrationBuilder.DropTable(
                name: "KnowledgeBases");
        }
    }
}
