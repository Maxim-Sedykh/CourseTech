using CourseTech.Domain.Constants.Validation;
using CourseTech.Domain.Dto.Auth;
using CourseTech.Domain.Extensions;
using FluentValidation;

namespace CourseTech.Application.Validations.FluentValidations.Auth;

/// <summary>
/// Валидация регистрации пользователя
/// </summary>
public class RegisterUserValidator : AbstractValidator<RegisterUserDto>
{
    public RegisterUserValidator()
    {
        RuleFor(x => x.Login).ValidateLogin();

        RuleFor(x => x.DateOfBirth)
            .NotEmpty().WithMessage(ValidationErrorMessages.DateOfBirthNotEmptyMessage)
            .Must(x => BeValidDateOfBirth(x)).WithMessage(ValidationErrorMessages.ValidDateOfBirthMessage);

        RuleFor(x => x.Password).ValidatePassword();

        RuleFor(x => x.PasswordConfirm)
            .NotEmpty().WithMessage(ValidationErrorMessages.PasswordConfirmNotEmptyMessage)
            .Equal(x => x.Password).WithMessage(ValidationErrorMessages.PasswordConfirmNotEqualPasswordMessage);
    }

    private bool BeValidDateOfBirth(DateTime dateOfBirth) => dateOfBirth < DateTime.Now;
}
