using CourseTech.ChatGptApi.Interfaces;
using CourseTech.ChatGptApi;
using CourseTech.DAL.Auth;
using CourseTech.DAL.Cache;
using CourseTech.DAL.DatabaseHelpers;
using CourseTech.DAL.Interceptors;
using CourseTech.DAL.UserQueryAnalyzers;
using CourseTech.DAL.Views;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Entities.QuestionEntities.QuestionTypesEntities;
using CourseTech.Domain.Interfaces.Cache;
using CourseTech.Domain.Interfaces.Databases;
using CourseTech.Domain.Interfaces.Helpers;
using CourseTech.Domain.Interfaces.UserQueryAnalyzers;
using CourseTech.Domain.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Role = CourseTech.Domain.Entities.UserRelated.Role;
using CourseTech.DAL.Repositories.Base;
using CourseTech.Domain.Entities.UserRelated;
using CourseTech.Domain.Interfaces.Repositories.Base;

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

        services.AddSingleton<ISqlQueryProvider, SqlQueryProvider>();

        services.AddScoped<IChatGptQueryAnalyzer, ChatGptQueryAnalyzer>();

        services.AddScoped<IChatGptService, ChatGptService>();

        services.InitCaching(configuration);

        services.InitEntityRepositories();
        services.InitViewRepositories();
        services.InitUnitOfWork();
    }

    /// <summary>
    /// Внедрение зависимостей репозиториев для сущностей
    /// </summary>
    /// <param name="services"></param>
    private static void InitEntityRepositories(this IServiceCollection services)
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
            typeof(OpenQuestionAnswer),
            typeof(Section),
            typeof(Session),
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

    /// <summary>
    /// Внедрение зависимостей репозиториев для представлений
    /// </summary>
    /// <param name="services"></param>
    private static void InitViewRepositories(this IServiceCollection services)
    {
        services.AddScoped<IViewRepository<QuestionTypeGrade>, ViewRepository<QuestionTypeGrade>>();
    }

    /// <summary>
    /// Внедрение зависимости для UoW
    /// </summary>
    /// <param name="services"></param>
    private static void InitUnitOfWork(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }

    /// <summary>
    /// Настройка кэширования
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    private static void InitCaching(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ICacheService, CacheService>();

        var redisSettings = configuration.GetSection("RedisSettings").Get<RedisSettings>();

        if (redisSettings == null || string.IsNullOrEmpty(redisSettings.Url))
        {
            throw new ArgumentNullException("Redis settings are not configured properly.");
        }

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = redisSettings.Url;
            options.InstanceName = redisSettings.InstanceName; 
        });
    }
}
