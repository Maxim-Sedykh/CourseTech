using CourseTech.Domain.Entities.UserRelated;
using CourseTech.Domain.Result;

namespace CourseTech.Domain.Interfaces.Validators;

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
