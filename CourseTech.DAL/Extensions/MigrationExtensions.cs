using CourseTech.DAL;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.DAL.Extensions
{
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

            dbContext.Database.Migrate();
        }
    }
}
