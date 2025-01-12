using CourseTech.Domain.Entities;
using CourseTech.Domain.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Domain.Interfaces.Validators
{
    /// <summary>
    /// Валидатор сервиса для работы с пользователями.
    /// </summary>
    public interface IUserValidator
    {
        /// <summary>
        /// Валидация удаления пользователя
        /// </summary>
        /// <param name="userProfile"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        BaseResult ValidateDeletingUser(UserProfile userProfile, User user);
    }
}
