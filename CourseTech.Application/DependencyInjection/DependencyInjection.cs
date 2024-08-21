using CourseTech.Application.Mapping;
using CourseTech.Application.Services;
using CourseTech.Domain.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CourseTech.Application.DependencyInjection;

/// <summary>
/// Внедрение зависимостей слоя Application
/// </summary>
public static class DependencyInjection
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.InitAutoMapper();

        services.InitServices();

        services.InitFluentValidators();
    }

    private static void InitServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ICourseResultService, CourseResultService>();
        services.AddScoped<ILessonRecordService, LessonRecordService>();
        services.AddScoped<ILessonService, LessonService>();
        services.AddScoped<IReviewService, ReviewService>();
        services.AddScoped<IUserProfileService, UserProfileService>();
        services.AddScoped<IUserService, UserService>();
    }

    private static void InitAutoMapper(this IServiceCollection services)
    {
        var validatorsTypes = new List<Type>()
        {
            typeof(ReviewMapping)
        };

        foreach (var validatorType in validatorsTypes)
        {
            services.AddAutoMapper(validatorType);
        }
    }

    public static void InitFluentValidators(this IServiceCollection services)
    {
        //var validatorsTypes = new List<Type>()
        //{
        //    typeof(CreateProductValidator),
        //    typeof(UpdateProductValidator),
        //    typeof(CreatePaymentValidator),
        //    typeof(UpdatePaymentValidation),
        //    typeof(UpdateOrderValidation),
        //    typeof(CreateOrderValidation),
        //    typeof(LoginUserValidator),
        //    typeof(RegisterUserValidation),
        //    typeof(CreateRoleValidation),
        //    typeof(DeleteUserRoleValidation),
        //    typeof(UpdateUserRoleValidation),
        //    typeof(UserRoleValidation),
        //    typeof(RoleValidation)
        //};

        //foreach (var validatorType in validatorsTypes)
        //{
        //    services.AddValidatorsFromAssembly(validatorType.Assembly);
        //}
    }
}
