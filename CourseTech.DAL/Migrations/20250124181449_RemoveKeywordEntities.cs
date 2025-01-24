using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourseTech.DAL.Migrations
{
    /// <inheritdoc />
    public partial class RemoveKeywordEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PracticalQuestionQueryKeyword");

            migrationBuilder.DropTable(
                name: "Keyword");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Keyword",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Content = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Keyword", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PracticalQuestionQueryKeyword",
                columns: table => new
                {
                    Number = table.Column<int>(type: "int", nullable: false),
                    KeywordId = table.Column<int>(type: "int", nullable: false),
                    PracticalQuestionId = table.Column<int>(type: "int", nullable: false),
                    IsStopWord = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PracticalQuestionQueryKeyword", x => new { x.Number, x.KeywordId, x.PracticalQuestionId });
                    table.ForeignKey(
                        name: "FK_PracticalQuestionQueryKeyword_BaseQuestion_PracticalQuestionId",
                        column: x => x.PracticalQuestionId,
                        principalTable: "BaseQuestion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PracticalQuestionQueryKeyword_Keyword_KeywordId",
                        column: x => x.KeywordId,
                        principalTable: "Keyword",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Keyword_Content",
                table: "Keyword",
                column: "Content",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PracticalQuestionQueryKeyword_KeywordId",
                table: "PracticalQuestionQueryKeyword",
                column: "KeywordId");

            migrationBuilder.CreateIndex(
                name: "IX_PracticalQuestionQueryKeyword_PracticalQuestionId",
                table: "PracticalQuestionQueryKeyword",
                column: "PracticalQuestionId");
        }
    }
}
