using CourseTech.ChatGptApi;
using CourseTech.ChatGptApi.Interfaces;
using CourseTech.DAL.Auth;
using CourseTech.DAL.Cache;
using CourseTech.DAL.Interceptors;
using CourseTech.DAL.Repositories;
using CourseTech.DAL.Repositories.Base;
using CourseTech.Domain.Interfaces.Cache;
using CourseTech.Domain.Interfaces.Databases;
using CourseTech.Domain.Interfaces.Helpers;
using CourseTech.Domain.Interfaces.Repositories;
using CourseTech.Domain.Settings;
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

        services.AddSingleton<IPasswordHasher, PasswordHasher>();

        services.AddScoped<IChatGptService, ChatGptService>();

        services.InitCaching(configuration);

        services.InitEntityRepositories();
        services.InitUnitOfWork();
    }

    /// <summary>
    /// Внедрение зависимостей репозиториев для сущностей
    /// </summary>
    /// <param name="services"></param>
    private static void InitEntityRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserProfileRepository, UserProfileRepository>();
        services.AddScoped<IUserTokenRepository, UserTokenRepository>();
        services.AddScoped<IAnswerRepository, AnswerRepository>();
        services.AddScoped<IQuestionRepository, QuestionRepository>();
        services.AddScoped<ISessionRepository, SessionRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
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
