using CourseTech.Domain.Dto.Role;
using CourseTech.Domain.Extensions;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Application.Validations.FluentValidations.Role
{
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
}
