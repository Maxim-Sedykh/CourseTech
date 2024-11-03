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
    /// Валидатор сервиса для работы с ролями.
    /// </summary>
    public interface IRoleValidator
    {
        /// <summary>
        /// Валидировать роли для пользователя
        /// </summary>
        /// <param name="user"></param>
        /// <param name="roles"></param>
        /// <returns></returns>
        BaseResult ValidateRoleForUser(User user, params Role[] roles);
    }
}
