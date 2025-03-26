using CourseTech.Domain.Dto.Role;
using CourseTech.Domain.Extensions;
using FluentValidation;

namespace CourseTech.Application.Validations.FluentValidations.Role;

/// <summary>
/// Валидация создания роли.
/// </summary>
public class CreateRoleValidator : AbstractValidator<CreateRoleDto>
{
    public CreateRoleValidator()
    {
        RuleFor(role => role.RoleName).ValidateRoleName();
    }
}
