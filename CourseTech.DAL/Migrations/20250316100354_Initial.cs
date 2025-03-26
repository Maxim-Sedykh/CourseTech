using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourseTech.DAL.Migrations;

/// <inheritdoc />
public partial class Initial : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Lesson",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Name = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                LessonType = table.Column<int>(type: "int", nullable: false),
                LectureMarkup = table.Column<string>(type: "nvarchar(max)", maxLength: 10000, nullable: true),
                Number = table.Column<int>(type: "int", nullable: false),
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
                Login = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_User", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "BaseQuestion",
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
                CorrectQueryCode = table.Column<string>(type: "nvarchar(max)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_BaseQuestion", x => x.Id);
                table.CheckConstraint("CK_Number", "Number BETWEEN 0 AND 100");
                table.ForeignKey(
                    name: "FK_BaseQuestion_Lesson_LessonId",
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
                Age = table.Column<int>(type: "int", nullable: false),
                DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                IsExamCompleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                CurrentGrade = table.Column<float>(type: "float(3)", precision: 3, scale: 2, nullable: false, defaultValue: 0f),
                LessonsCompleted = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                Analys = table.Column<string>(type: "nvarchar(max)", nullable: true, defaultValue: "Анализ ещё не получен. вы ещё не прошли курс."),
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
                OpenQuestionId = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_OpenQuestionAnswer", x => x.Id);
                table.ForeignKey(
                    name: "FK_OpenQuestionAnswer_BaseQuestion_OpenQuestionId",
                    column: x => x.OpenQuestionId,
                    principalTable: "BaseQuestion",
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
                Content = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                IsCorrect = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_TestVariant", x => x.Id);
                table.ForeignKey(
                    name: "FK_TestVariant_BaseQuestion_TestQuestionId",
                    column: x => x.TestQuestionId,
                    principalTable: "BaseQuestion",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_BaseQuestion_Id_Number",
            table: "BaseQuestion",
            columns: new[] { "Id", "Number" },
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_BaseQuestion_LessonId",
            table: "BaseQuestion",
            column: "LessonId");

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
            name: "IX_Review_UserId",
            table: "Review",
            column: "UserId");

        migrationBuilder.CreateIndex(
            name: "IX_TestVariant_TestQuestionId_VariantNumber",
            table: "TestVariant",
            columns: new[] { "TestQuestionId", "VariantNumber" },
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
            name: "BaseQuestion");

        migrationBuilder.DropTable(
            name: "Role");

        migrationBuilder.DropTable(
            name: "User");

        migrationBuilder.DropTable(
            name: "Lesson");
    }
}
