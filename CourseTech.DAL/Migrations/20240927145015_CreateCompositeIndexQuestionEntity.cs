using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourseTech.DAL.Migrations
{
    /// <inheritdoc />
    public partial class CreateCompositeIndexQuestionEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Question_Number",
                table: "Question");

            migrationBuilder.CreateIndex(
                name: "IX_Question_Id_Number",
                table: "Question",
                columns: new[] { "Id", "Number" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Question_Id_Number",
                table: "Question");

            migrationBuilder.CreateIndex(
                name: "IX_Question_Number",
                table: "Question",
                column: "Number",
                unique: true);
        }
    }
}
