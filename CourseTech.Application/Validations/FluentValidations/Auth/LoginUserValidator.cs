using CourseTech.Domain.Dto.Auth;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Application.Validations.FluentValidations.Auth
{
    public class LoginUserValidator : AbstractValidator<LoginUserDto>
    {
        public LoginUserValidator()
        {
            RuleFor(user => user.Login)
            .NotEmpty().WithMessage("Введите логин")
            .MinimumLength(4).WithMessage("Длина логина должна быть больше четырёх символов")
            .MaximumLength(20).WithMessage("Длина логина должна быть меньше 20 символов");

            RuleFor(user => user.Password)
                .NotEmpty().WithMessage("Введите пароль")
                .MinimumLength(5).WithMessage("Длина пароля должна быть больше пяти символов")
                .MaximumLength(20).WithMessage("Длина пароля должна быть меньше двадцати символов");
        }
    }
}
