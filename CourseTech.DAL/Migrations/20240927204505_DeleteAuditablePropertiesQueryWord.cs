using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourseTech.DAL.Migrations
{
    /// <inheritdoc />
    public partial class DeleteAuditablePropertiesQueryWord : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "QueryWord");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "QueryWord");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "QueryWord",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "QueryWord",
                type: "datetime2",
                nullable: true);
        }
    }
}
