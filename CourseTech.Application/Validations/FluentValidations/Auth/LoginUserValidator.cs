using CourseTech.Domain.Dto.Auth;
using CourseTech.Domain.Extensions;
using FluentValidation;

namespace CourseTech.Application.Validations.FluentValidations.Auth;

/// <summary>
/// Валидация авторизации пользователя
/// </summary>
public class LoginUserValidator : AbstractValidator<LoginUserDto>
{
    public LoginUserValidator()
    {
        RuleFor(x => x.Login).ValidateLogin();

        RuleFor(x => x.Password).ValidatePassword();
    }
}
