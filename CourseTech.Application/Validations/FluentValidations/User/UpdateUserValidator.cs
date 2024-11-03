using CourseTech.Domain.Dto.Role;
using CourseTech.Domain.Dto.User;
using CourseTech.Domain.Extensions;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Application.Validations.FluentValidations.User
{
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
}
