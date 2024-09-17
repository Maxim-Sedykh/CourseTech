using CourseTech.Domain.Dto.Review;
using CourseTech.Domain.Dto.Role;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Application.Validations.FluentValidations.Role
{
    public class CreateRoleValidator : AbstractValidator<CreateRoleDto>
    {
        public CreateRoleValidator()
        {
            RuleFor(role => role.Name)
            .NotEmpty().WithMessage("Введите имя роли")
            .MinimumLength(2).WithMessage("Длина роли должна быть больше 2 символов")
            .MaximumLength(50).WithMessage("Длина роли должна быть меньше 50 символов");
        }
    }
}
