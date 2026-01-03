using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace CourseTech.DAL.Extensions;

/// <summary>
/// Расширение IApplicationBuilder для запуска миграций Entity Framework Core из точки входа в приложение ( класс Program )
/// </summary>
public static class MigrationExtensions
{
    /// <summary>
    /// Применить все миграции базы данных
    /// </summary>
    /// <param name="app"></param>
    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        using IServiceScope score = app.ApplicationServices.CreateScope();

        using CourseDbContext dbContext = score.ServiceProvider.GetService<CourseDbContext>();

        //dbContext.Database.Migrate();
    }
}
