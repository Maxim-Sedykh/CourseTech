using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourseTech.DAL.Migrations
{
    /// <inheritdoc />
    public partial class CreateCompositeIndexTestVariantEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TestVariant_TestQuestionId",
                table: "TestVariant");

            migrationBuilder.DropIndex(
                name: "IX_TestVariant_VariantNumber",
                table: "TestVariant");

            migrationBuilder.CreateIndex(
                name: "IX_TestVariant_TestQuestionId_VariantNumber",
                table: "TestVariant",
                columns: new[] { "TestQuestionId", "VariantNumber" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TestVariant_TestQuestionId_VariantNumber",
                table: "TestVariant");

            migrationBuilder.CreateIndex(
                name: "IX_TestVariant_TestQuestionId",
                table: "TestVariant",
                column: "TestQuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_TestVariant_VariantNumber",
                table: "TestVariant",
                column: "VariantNumber",
                unique: true);
        }
    }
}
