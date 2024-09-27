using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourseTech.DAL.Migrations
{
    /// <inheritdoc />
    public partial class RenameTestVariantIsRightColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsRight",
                table: "TestVariant",
                newName: "IsCorrect");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsCorrect",
                table: "TestVariant",
                newName: "IsRight");
        }
    }
}
