using CourseTech.Domain.Extensions;
using FluentValidation;

namespace CourseTech.Application.Validations.FluentValidations.Role;

/// <summary>
/// Валидация обновления роли.
/// </summary>
public class UpdateRoleValidator : AbstractValidator<RoleDto>
{
    public UpdateRoleValidator()
    {
        RuleFor(role => role.RoleName).ValidateRoleName();
    }
}
