using CourseTech.DAL.Interceptors;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Entities.QuestionEntities;
using CourseTech.Domain.Entities.QuestionEntities.QuestionTypesEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Reflection;

namespace CourseTech.DAL;

public class CourseDbContext : DbContext
{
    #region Db tables

    public DbSet<OpenQuestion> OpenQuestions { get; set; }
    public DbSet<TestQuestion> TestQuestions { get; set; }
    public DbSet<PracticalQuestion> PracticalQuestions { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Lesson> Lessons { get; set; }
    public DbSet<LessonRecord> LessonRecords { get; set; }
    public DbSet<Keyword> Keywords { get; set; }
    public DbSet<OpenQuestionAnswer> OpenQuestionAnswers { get; set; }
    public DbSet<QueryWord> QueryWords { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<TestVariant> TestVariants { get; set; }
    public DbSet<UserProfile> UserProfiles { get; set; }
    public DbSet<UserToken> UserTokens { get; set; }
    public DbSet<Question> Questions { get; set; }

    #endregion

    public CourseDbContext(DbContextOptions<CourseDbContext> options) : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(new AuditInterceptor());

        optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
