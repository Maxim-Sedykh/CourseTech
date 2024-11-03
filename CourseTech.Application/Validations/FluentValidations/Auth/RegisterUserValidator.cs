using CourseTech.Domain.Dto.Auth;
using CourseTech.Domain.Extensions;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Application.Validations.FluentValidations.Auth
{
    /// <summary>
    /// Валидация регистрации пользователя
    /// </summary>
    public class RegisterUserValidator : AbstractValidator<RegisterUserDto>
    {
        public RegisterUserValidator()
        {
            RuleFor(x => x.Login).ValidateLogin();

            RuleFor(x => x.UserName).ValidateUserName();

            RuleFor(x => x.Surname).ValidateUserSurname();

            RuleFor(x => x.DateOfBirth)
                .NotEmpty().WithMessage("Введите дату рождения")
                .Must(BeValidDateOfBirth).WithMessage("Неверный формат даты рождения");

            RuleFor(x => x.Password).ValidatePassword();

            RuleFor(x => x.PasswordConfirm)
                .NotEmpty().WithMessage("Подтвердите пароль")
                .Equal(x => x.Password).WithMessage("Пароли не совпадают");
        }

        private bool BeValidDateOfBirth(DateTime dateOfBirth) => dateOfBirth < DateTime.Now;

        private bool HaveAtLeastOneDigit(string password) => password.Any(char.IsDigit);

        private bool HaveAtLeastOneUppercase(string password) => password.Any(char.IsUpper);
    }
}
