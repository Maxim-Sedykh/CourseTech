using CourseTech.DAL.Interceptors;
using CourseTech.DAL.Repositories;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Interfaces.Databases;
using CourseTech.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CourseTech.DAL.DependencyInjection;

public static class DependencyInjection
{
    /// <summary>
    /// Внедрение зависимостей слоя DAL
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    public static void AddDataAccessLayer(this IServiceCollection services, IConfiguration configuration)
    {
        var courseConnectionString = configuration.GetConnectionString("CourseDbConnection");

        services.AddSingleton<AuditInterceptor>();
        services.AddDbContext<CourseDbContext>(options =>
        {
            options.UseSqlServer(courseConnectionString);
        });

        services.InitRepositories();
        services.InitUnitOfWork();
    }

    private static void InitRepositories(this IServiceCollection services)
    {
        var types = new List<Type>()
        {
            typeof(User),
            typeof(Keyword),
            typeof(Lesson),
            typeof(LessonRecord),
            typeof(UserToken),
            typeof(UserRole),
            typeof(Role),
            typeof(QueryWord),
            typeof(Question),
            typeof(Review),
            typeof(TestVariant),
            typeof(UserProfile)
        };

        foreach (var type in types)
        {
            var interfaceType = typeof(IBaseRepository<>).MakeGenericType(type);
            var implementationType = typeof(BaseRepository<>).MakeGenericType(type);
            services.AddScoped(interfaceType, implementationType);
        }
    }

    private static void InitUnitOfWork(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }
}
