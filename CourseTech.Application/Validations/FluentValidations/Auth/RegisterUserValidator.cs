using CourseTech.Domain.Dto.Auth;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Application.Validations.FluentValidations.Auth
{
    public class RegisterUserValidator : AbstractValidator<RegisterUserDto>
    {
        public RegisterUserValidator()
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

            RuleFor(x => x.DateOfBirth)
                .NotEmpty().WithMessage("Введите дату рождения")
                .Must(BeValidDateOfBirth).WithMessage("Неверный формат даты рождения");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Введите пароль")
                .MinimumLength(8).WithMessage("Пароль должен быть не менее 8 символов");

            RuleFor(x => x.PasswordConfirm)
                .NotEmpty().WithMessage("Подтвердите пароль")
                .Equal(x => x.Password).WithMessage("Пароли не совпадают");
        }

        private bool BeValidDateOfBirth(DateTime dateOfBirth) => dateOfBirth < DateTime.Now;

        private bool HaveAtLeastOneDigit(string password) => password.Any(char.IsDigit);

        private bool HaveAtLeastOneUppercase(string password) => password.Any(char.IsUpper);
    }
}
