using CourseTech.Domain.Interfaces.Dtos.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Domain.Dto.UserProfile
{
    /// <summary>
    /// Модель данных для обновления пользователем своих учётных данных в профиле.
    /// </summary>
    public class UpdateUserProfileDto: IUserNameValidation, ISurnameValidation
    {
        public string UserName { get; set; }

        public string Surname { get; set; }

        public DateTime DateOfBirth { get; set; }
    }
}
