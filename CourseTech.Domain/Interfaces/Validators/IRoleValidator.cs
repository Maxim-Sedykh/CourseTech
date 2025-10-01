using CourseTech.Domain.Entities.UserRelated;
using CourseTech.Domain.Result;

namespace CourseTech.Domain.Interfaces.Validators;

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
