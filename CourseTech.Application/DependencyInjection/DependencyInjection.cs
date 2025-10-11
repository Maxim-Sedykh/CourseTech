using CourseTech.Application.Services;
using CourseTech.Application.Validations.FluentValidations.Auth;
using CourseTech.Application.Validations.FluentValidations.User;
using CourseTech.Application.Validations.Validators;
using CourseTech.Domain.Interfaces.Services;
using CourseTech.Domain.Interfaces.Validators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace CourseTech.Application.DependencyInjection;

/// <summary>
/// Класс для внедрения зависимостей слоя Application
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Внедрение зависимостей слоя Application
    /// </summary>
    /// <param name="services"></param>
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cf => cf.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));

        services.InitServices();

        services.InitFluentValidators();

        services.InitEntityValidators();
    }

    /// <summary>
    /// Зарегистрировать зависимости для сервисов слоя Application.
    /// </summary>
    /// <param name="services"></param>
    private static void InitServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserProfileService, UserProfileService>();
        services.AddScoped<IQuestionService, QuestionService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IUserService, UserService>();
    }

    /// <summary>
    /// Настройка валидации с помощью библиотеки FluentValidation.
    /// </summary>
    /// <param name="services"></param>
    public static void InitFluentValidators(this IServiceCollection services)
    {
        var validatorsTypes = new List<Type>()
        {
            typeof(LoginUserValidator),
            typeof(RegisterUserValidator),
            typeof(UpdateUserValidator)
        };

        foreach (var validatorType in validatorsTypes)
        {
            services.AddValidatorsFromAssembly(validatorType.Assembly);
        }
    }

    /// <summary>
    /// Настройка зависимостей валидаторов для сервисов.
    /// </summary>
    /// <param name="services"></param>
    public static void InitEntityValidators(this IServiceCollection services)
    {
        services.AddScoped<IAuthValidator, AuthValidator>();
        services.AddScoped<IUserValidator, UserValidator>();
    }
}
