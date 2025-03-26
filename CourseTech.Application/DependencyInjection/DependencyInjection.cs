using CourseTech.Application.Helpers;
using CourseTech.Application.Mapping;
using CourseTech.Application.Services;
using CourseTech.Application.Validations.FluentValidations.Auth;
using CourseTech.Application.Validations.FluentValidations.Lesson;
using CourseTech.Application.Validations.FluentValidations.Review;
using CourseTech.Application.Validations.FluentValidations.Role;
using CourseTech.Application.Validations.FluentValidations.User;
using CourseTech.Application.Validations.Validators;
using CourseTech.Domain.Interfaces.Helpers;
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
        services.InitAutoMapper();

        services.AddMediatR(cf => cf.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));

        services.InitServices();

        services.InitFluentValidators();

        services.InitEntityValidators();

        services.AddScoped<IQuestionAnswerChecker, QuestionAnswerChecker>();
    }

    /// <summary>
    /// Зарегистрировать зависимости для сервисов слоя Application.
    /// </summary>
    /// <param name="services"></param>
    private static void InitServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ICourseResultService, CourseResultService>();
        services.AddScoped<ILessonRecordService, LessonRecordService>();
        services.AddScoped<ILessonService, LessonService>();
        services.AddScoped<IReviewService, ReviewService>();
        services.AddScoped<IUserProfileService, UserProfileService>();
        services.AddScoped<IQuestionService, QuestionService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IUserService, UserService>();
    }

    /// <summary>
    /// Настройка AutoMapper.
    /// </summary>
    /// <param name="services"></param>
    private static void InitAutoMapper(this IServiceCollection services)
    {
        var mappingTypes = new List<Type>()
        {
            typeof(ReviewMapping),
            typeof(LessonMapping),
            typeof(LessonRecordMapping),
            typeof(QuestionMapping),
            typeof(RoleMapping),
            typeof(UserProfileMapping),
            typeof(UserMapping)
        };

        foreach (var mappingType in mappingTypes)
        {
            services.AddAutoMapper(mappingType);
        }
    }

    /// <summary>
    /// Настройка валидации с помощью библиотеки FluentValidation.
    /// </summary>
    /// <param name="services"></param>
    public static void InitFluentValidators(this IServiceCollection services)
    {
        var validatorsTypes = new List<Type>()
        {
            typeof(CreateReviewValidator),
            typeof(CreateRoleValidator),
            typeof(LoginUserValidator),
            typeof(RegisterUserValidator),
            typeof(LessonLectureValidator),
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
        services.AddScoped<IQuestionValidator, QuestionValidator>();
        services.AddScoped<IRoleValidator, RoleValidator>();
        services.AddScoped<IUserValidator, UserValidator>();
        services.AddScoped<ILessonValidator, LessonValidator>();
        services.AddScoped<ICourseResultValidator, CourseResultValidator>();
    }
}
