using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourseTech.DAL.Migrations
{
    /// <inheritdoc />
    public partial class ChangeDateOfBirthColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DateOfBirth",
                table: "UserProfile",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("0f8fad5b-d9cb-469f-a165-70867728950e"),
                column: "CreatedAt",
                value: new DateTime(2024, 9, 18, 13, 14, 23, 315, DateTimeKind.Utc).AddTicks(2732));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("7c9e6679-7425-40de-944b-e07fc1f90ae7"),
                column: "CreatedAt",
                value: new DateTime(2024, 9, 18, 13, 14, 23, 315, DateTimeKind.Utc).AddTicks(2729));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("9245fe4a-d402-451c-b9ed-9c1a04247482"),
                column: "CreatedAt",
                value: new DateTime(2024, 9, 18, 13, 14, 23, 315, DateTimeKind.Utc).AddTicks(2723));

            migrationBuilder.UpdateData(
                table: "UserProfile",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "CreatedAt", "DateOfBirth" },
                values: new object[] { new DateTime(2024, 9, 18, 13, 14, 23, 317, DateTimeKind.Utc).AddTicks(7453), new DateTime(2002, 2, 2, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "UserProfile",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "CreatedAt", "DateOfBirth" },
                values: new object[] { new DateTime(2024, 9, 18, 13, 14, 23, 317, DateTimeKind.Utc).AddTicks(7459), new DateTime(2006, 7, 5, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "UserProfile",
                keyColumn: "Id",
                keyValue: 3L,
                columns: new[] { "CreatedAt", "DateOfBirth" },
                values: new object[] { new DateTime(2024, 9, 18, 13, 14, 23, 317, DateTimeKind.Utc).AddTicks(7463), new DateTime(1980, 3, 2, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "UserToken",
                keyColumn: "Id",
                keyValue: 1L,
                column: "RefreshTokenExpireTime",
                value: new DateTime(2024, 9, 25, 13, 14, 23, 318, DateTimeKind.Utc).AddTicks(5108));

            migrationBuilder.UpdateData(
                table: "UserToken",
                keyColumn: "Id",
                keyValue: 2L,
                column: "RefreshTokenExpireTime",
                value: new DateTime(2024, 9, 25, 13, 14, 23, 318, DateTimeKind.Utc).AddTicks(5120));

            migrationBuilder.UpdateData(
                table: "UserToken",
                keyColumn: "Id",
                keyValue: 3L,
                column: "RefreshTokenExpireTime",
                value: new DateTime(2024, 9, 25, 13, 14, 23, 318, DateTimeKind.Utc).AddTicks(5123));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateOnly>(
                name: "DateOfBirth",
                table: "UserProfile",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("0f8fad5b-d9cb-469f-a165-70867728950e"),
                column: "CreatedAt",
                value: new DateTime(2024, 9, 18, 10, 24, 44, 60, DateTimeKind.Utc).AddTicks(6014));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("7c9e6679-7425-40de-944b-e07fc1f90ae7"),
                column: "CreatedAt",
                value: new DateTime(2024, 9, 18, 10, 24, 44, 60, DateTimeKind.Utc).AddTicks(6011));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("9245fe4a-d402-451c-b9ed-9c1a04247482"),
                column: "CreatedAt",
                value: new DateTime(2024, 9, 18, 10, 24, 44, 60, DateTimeKind.Utc).AddTicks(6002));

            migrationBuilder.UpdateData(
                table: "UserProfile",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "CreatedAt", "DateOfBirth" },
                values: new object[] { new DateTime(2024, 9, 18, 10, 24, 44, 63, DateTimeKind.Utc).AddTicks(8307), new DateOnly(2002, 2, 2) });

            migrationBuilder.UpdateData(
                table: "UserProfile",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "CreatedAt", "DateOfBirth" },
                values: new object[] { new DateTime(2024, 9, 18, 10, 24, 44, 63, DateTimeKind.Utc).AddTicks(8316), new DateOnly(2006, 7, 5) });

            migrationBuilder.UpdateData(
                table: "UserProfile",
                keyColumn: "Id",
                keyValue: 3L,
                columns: new[] { "CreatedAt", "DateOfBirth" },
                values: new object[] { new DateTime(2024, 9, 18, 10, 24, 44, 63, DateTimeKind.Utc).AddTicks(8320), new DateOnly(1980, 3, 2) });

            migrationBuilder.UpdateData(
                table: "UserToken",
                keyColumn: "Id",
                keyValue: 1L,
                column: "RefreshTokenExpireTime",
                value: new DateTime(2024, 9, 25, 10, 24, 44, 65, DateTimeKind.Utc).AddTicks(8590));

            migrationBuilder.UpdateData(
                table: "UserToken",
                keyColumn: "Id",
                keyValue: 2L,
                column: "RefreshTokenExpireTime",
                value: new DateTime(2024, 9, 25, 10, 24, 44, 65, DateTimeKind.Utc).AddTicks(8609));

            migrationBuilder.UpdateData(
                table: "UserToken",
                keyColumn: "Id",
                keyValue: 3L,
                column: "RefreshTokenExpireTime",
                value: new DateTime(2024, 9, 25, 10, 24, 44, 65, DateTimeKind.Utc).AddTicks(8612));
        }
    }
}
