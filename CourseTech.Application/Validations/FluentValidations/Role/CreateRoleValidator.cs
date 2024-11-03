using CourseTech.Domain.Dto.Review;
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
    /// Валидация создания роли.
    /// </summary>
    public class CreateRoleValidator : AbstractValidator<CreateRoleDto>
    {
        public CreateRoleValidator()
        {
            RuleFor(role => role.RoleName).ValidateRoleName();
        }
    }
}
