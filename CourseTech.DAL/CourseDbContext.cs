using CourseTech.DAL.Interceptors;
using CourseTech.Domain.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace CourseTech.DAL;

/// <summary>
/// Контекст основной базы данных, связанной с курсом
/// </summary>
public class CourseDbContext : DbContext
{
    public DbSet<QuestionTypeGrade> QuestionTypeGrades { get; set; }

    public CourseDbContext(DbContextOptions options) : base(options) { }

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
