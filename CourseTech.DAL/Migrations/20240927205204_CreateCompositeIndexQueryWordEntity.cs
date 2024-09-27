using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourseTech.DAL.Migrations
{
    /// <inheritdoc />
    public partial class CreateCompositeIndexQueryWordEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_QueryWord_Number",
                table: "QueryWord");

            migrationBuilder.CreateIndex(
                name: "IX_QueryWord_Id_Number",
                table: "QueryWord",
                columns: new[] { "Id", "Number" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_QueryWord_Id_Number",
                table: "QueryWord");

            migrationBuilder.CreateIndex(
                name: "IX_QueryWord_Number",
                table: "QueryWord",
                column: "Number",
                unique: true);
        }
    }
}
