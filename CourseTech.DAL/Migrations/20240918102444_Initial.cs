using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CourseTech.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Keyword",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Word = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Keyword", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Lesson",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(75)", maxLength: 75, nullable: false),
                    LessonType = table.Column<int>(type: "int", nullable: false),
                    LectureMarkup = table.Column<string>(type: "nvarchar(max)", maxLength: 10000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lesson", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Login = table.Column<string>(type: "nvarchar(18)", maxLength: 18, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Question",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Number = table.Column<int>(type: "int", nullable: false),
                    LessonId = table.Column<int>(type: "int", nullable: false),
                    DisplayQuestion = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Discriminator = table.Column<string>(type: "nvarchar(21)", maxLength: 21, nullable: false),
                    Notation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CorrectQueryCode = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Question", x => x.Id);
                    table.CheckConstraint("CK_Question_Number", "Number BETWEEN 0 AND 100");
                    table.ForeignKey(
                        name: "FK_Question_Lesson_LessonId",
                        column: x => x.LessonId,
                        principalTable: "Lesson",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LessonRecord",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LessonId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Mark = table.Column<float>(type: "real", nullable: false, defaultValue: 0f),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LessonRecord", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LessonRecord_Lesson_LessonId",
                        column: x => x.LessonId,
                        principalTable: "Lesson",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LessonRecord_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Review",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReviewText = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Review", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Review_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserProfile",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Age = table.Column<byte>(type: "tinyint", nullable: false),
                    DateOfBirth = table.Column<DateOnly>(type: "date", nullable: false),
                    IsExamCompleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CurrentGrade = table.Column<float>(type: "float(3)", precision: 3, scale: 2, nullable: false, defaultValue: 0f),
                    LessonsCompleted = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Analys = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsEditAble = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CountOfReviews = table.Column<byte>(type: "tinyint", nullable: false, defaultValue: (byte)0),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProfile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserProfile_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRole",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRole", x => new { x.RoleId, x.UserId });
                    table.ForeignKey(
                        name: "FK_UserRole_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRole_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserToken",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RefreshTokenExpireTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserToken", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserToken_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OpenQuestionAnswer",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AnswerText = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    OpenQuestionId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpenQuestionAnswer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OpenQuestionAnswer_Question_OpenQuestionId",
                        column: x => x.OpenQuestionId,
                        principalTable: "Question",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QueryWord",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Number = table.Column<int>(type: "int", nullable: false),
                    KeywordId = table.Column<int>(type: "int", nullable: false),
                    PracticalQuestionId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QueryWord", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QueryWord_Keyword_KeywordId",
                        column: x => x.KeywordId,
                        principalTable: "Keyword",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QueryWord_Question_PracticalQuestionId",
                        column: x => x.PracticalQuestionId,
                        principalTable: "Question",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TestVariant",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TestQuestionId = table.Column<int>(type: "int", nullable: false),
                    VariantNumber = table.Column<int>(type: "int", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsRight = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestVariant", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TestVariant_Question_TestQuestionId",
                        column: x => x.TestQuestionId,
                        principalTable: "Question",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "CreatedAt", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { 1L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "User", null },
                    { 2L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin", null },
                    { 3L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Moderator", null }
                });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "CreatedAt", "Login", "Password", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("0f8fad5b-d9cb-469f-a165-70867728950e"), new DateTime(2024, 9, 18, 10, 24, 44, 60, DateTimeKind.Utc).AddTicks(6014), "Sasha_student002", "------------------------------------------", null },
                    { new Guid("7c9e6679-7425-40de-944b-e07fc1f90ae7"), new DateTime(2024, 9, 18, 10, 24, 44, 60, DateTimeKind.Utc).AddTicks(6011), "Maximkaboss25", "------------------------------------------", null },
                    { new Guid("9245fe4a-d402-451c-b9ed-9c1a04247482"), new DateTime(2024, 9, 18, 10, 24, 44, 60, DateTimeKind.Utc).AddTicks(6002), "MainAdmin", "------------------------------------------", null }
                });

            migrationBuilder.InsertData(
                table: "UserProfile",
                columns: new[] { "Id", "Age", "Analys", "CreatedAt", "DateOfBirth", "Name", "Surname", "UpdatedAt", "UserId" },
                values: new object[,]
                {
                    { 1L, (byte)0, null, new DateTime(2024, 9, 18, 10, 24, 44, 63, DateTimeKind.Utc).AddTicks(8307), new DateOnly(2002, 2, 2), "Админ", "Админов", null, new Guid("9245fe4a-d402-451c-b9ed-9c1a04247482") },
                    { 2L, (byte)0, null, new DateTime(2024, 9, 18, 10, 24, 44, 63, DateTimeKind.Utc).AddTicks(8316), new DateOnly(2006, 7, 5), "Максим", "Максимов", null, new Guid("7c9e6679-7425-40de-944b-e07fc1f90ae7") },
                    { 3L, (byte)0, null, new DateTime(2024, 9, 18, 10, 24, 44, 63, DateTimeKind.Utc).AddTicks(8320), new DateOnly(1980, 3, 2), "Александра", "Александрова", null, new Guid("0f8fad5b-d9cb-469f-a165-70867728950e") }
                });

            migrationBuilder.InsertData(
                table: "UserRole",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { 1L, new Guid("0f8fad5b-d9cb-469f-a165-70867728950e") },
                    { 1L, new Guid("7c9e6679-7425-40de-944b-e07fc1f90ae7") },
                    { 2L, new Guid("9245fe4a-d402-451c-b9ed-9c1a04247482") },
                    { 3L, new Guid("7c9e6679-7425-40de-944b-e07fc1f90ae7") }
                });

            migrationBuilder.InsertData(
                table: "UserToken",
                columns: new[] { "Id", "RefreshToken", "RefreshTokenExpireTime", "UserId" },
                values: new object[,]
                {
                    { 1L, "jbodfiujbINOIU3O4$", new DateTime(2024, 9, 25, 10, 24, 44, 65, DateTimeKind.Utc).AddTicks(8590), new Guid("9245fe4a-d402-451c-b9ed-9c1a04247482") },
                    { 2L, "hgiroej[giertjivfs", new DateTime(2024, 9, 25, 10, 24, 44, 65, DateTimeKind.Utc).AddTicks(8609), new Guid("7c9e6679-7425-40de-944b-e07fc1f90ae7") },
                    { 3L, "reatbyt42t423hgerf", new DateTime(2024, 9, 25, 10, 24, 44, 65, DateTimeKind.Utc).AddTicks(8612), new Guid("0f8fad5b-d9cb-469f-a165-70867728950e") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_LessonRecord_LessonId",
                table: "LessonRecord",
                column: "LessonId");

            migrationBuilder.CreateIndex(
                name: "IX_LessonRecord_UserId",
                table: "LessonRecord",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_OpenQuestionAnswer_OpenQuestionId",
                table: "OpenQuestionAnswer",
                column: "OpenQuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_QueryWord_KeywordId",
                table: "QueryWord",
                column: "KeywordId");

            migrationBuilder.CreateIndex(
                name: "IX_QueryWord_Number",
                table: "QueryWord",
                column: "Number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_QueryWord_PracticalQuestionId",
                table: "QueryWord",
                column: "PracticalQuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_Question_LessonId",
                table: "Question",
                column: "LessonId");

            migrationBuilder.CreateIndex(
                name: "IX_Question_Number",
                table: "Question",
                column: "Number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Review_UserId",
                table: "Review",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TestVariant_TestQuestionId",
                table: "TestVariant",
                column: "TestQuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_TestVariant_VariantNumber",
                table: "TestVariant",
                column: "VariantNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserProfile_UserId",
                table: "UserProfile",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_UserId",
                table: "UserRole",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserToken_UserId",
                table: "UserToken",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LessonRecord");

            migrationBuilder.DropTable(
                name: "OpenQuestionAnswer");

            migrationBuilder.DropTable(
                name: "QueryWord");

            migrationBuilder.DropTable(
                name: "Review");

            migrationBuilder.DropTable(
                name: "TestVariant");

            migrationBuilder.DropTable(
                name: "UserProfile");

            migrationBuilder.DropTable(
                name: "UserRole");

            migrationBuilder.DropTable(
                name: "UserToken");

            migrationBuilder.DropTable(
                name: "Keyword");

            migrationBuilder.DropTable(
                name: "Question");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Lesson");
        }
    }
}
