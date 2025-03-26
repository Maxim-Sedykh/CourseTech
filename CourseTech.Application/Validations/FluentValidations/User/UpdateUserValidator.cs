using CourseTech.Domain.Dto.User;
using CourseTech.Domain.Extensions;
using FluentValidation;

namespace CourseTech.Application.Validations.FluentValidations.User;

/// <summary>
/// Валидация для обновления информации о пользователе (Admin Dashboard).
/// </summary>
public class UpdateUserValidator : AbstractValidator<UpdateUserDto>
{
    public UpdateUserValidator()
    {
        RuleFor(x => x.Login).ValidateLogin();

        RuleFor(x => x.UserName).ValidateUserName();

        RuleFor(x => x.Surname).ValidateUserSurname();
    }
}
