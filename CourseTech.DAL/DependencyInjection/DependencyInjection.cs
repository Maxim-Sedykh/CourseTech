using CourseTech.DAL.Auth;
using CourseTech.DAL.Cache;
using CourseTech.DAL.Graph;
using CourseTech.DAL.Interceptors;
using CourseTech.DAL.Repositories;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Entities.QuestionEntities;
using CourseTech.Domain.Entities.QuestionEntities.QuestionTypesEntities;
using CourseTech.Domain.Helpers;
using CourseTech.Domain.Interfaces.Cache;
using CourseTech.Domain.Interfaces.Databases;
using CourseTech.Domain.Interfaces.Graph;
using CourseTech.Domain.Interfaces.Helpers;
using CourseTech.Domain.Interfaces.Repositories;
using CourseTech.Domain.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using Role = CourseTech.Domain.Entities.Role;

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

        services.AddSingleton<IPasswordHasher, PasswordHasher>();

        services.AddScoped<IQueryGraphAnalyzer, QueryGraphAnalyzer>();

        services.AddSingleton<ISqlHelper, SqlHelper>();

        services.InitCaching(configuration);

        services.InitRepositories();
        services.InitUnitOfWork();
    }
     
    private static void InitRepositories(this IServiceCollection services)
    {
        var types = new List<Type>()
        {
            typeof(UserToken),
            typeof(User),
            typeof(UserRole),
            typeof(UserProfile),
            typeof(TestVariant),
            typeof(Review),
            typeof(Role),
            typeof(QueryWord),
            typeof(OpenQuestionAnswer),
            typeof(Lesson),
            typeof(LessonRecord),
            typeof(Keyword),
            typeof(Question),
            typeof(OpenQuestion),
            typeof(TestQuestion),
            typeof(PracticalQuestion)
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

    private static void InitCaching(this IServiceCollection services, IConfiguration configuration)
    {
        // To Do убрать хард код
        services.AddScoped<ICacheService, CacheService>();

        var redisConfig = configuration.GetSection(nameof(RedisSettings));
        var redisUrl = redisConfig["Url"];

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetConnectionString("Cache");
        });

        services.AddScoped(cfg =>
        {
            var multiplexer = ConnectionMultiplexer.Connect(redisUrl ?? string.Empty);
            return multiplexer.GetDatabase();
        });
    }
}
