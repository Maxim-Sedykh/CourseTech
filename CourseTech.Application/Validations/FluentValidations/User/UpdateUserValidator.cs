using CourseTech.Domain.Dto.Role;
using CourseTech.Domain.Dto.User;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Application.Validations.FluentValidations.User
{
    public class UpdateUserValidator : AbstractValidator<UpdateUserDto>
    {
        public UpdateUserValidator()
        {
            RuleFor(x => x.Login)
               .NotEmpty().WithMessage("Введите логин")
               .MinimumLength(4).WithMessage("Логин должен быть не менее 4 символов")
               .MaximumLength(20).WithMessage("Логин должен быть не более 20 символов");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Введите имя")
                .Matches(@"^[А-Яа-я]+$").WithMessage("Имя должно содержать только русские буквы");

            RuleFor(x => x.Surname)
                .NotEmpty().WithMessage("Введите фамилию")
                .Matches(@"^[А-Яа-я]+$").WithMessage("Фамилия должна содержать только русские буквы");
        }
    }
}
