using CourseTech.DAL;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Domain.Extensions
{
    public static class MigrationExtensions
    {
        public static void ApplyMigrations(this IApplicationBuilder app)
        {
            using IServiceScope score = app.ApplicationServices.CreateScope();

            using CourseDbContext dbContext = score.ServiceProvider.GetService<CourseDbContext>();

            //dbContext.Database.Migrate();
        }
    }
}
