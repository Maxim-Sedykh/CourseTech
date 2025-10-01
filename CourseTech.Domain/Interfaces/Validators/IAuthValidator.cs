using CourseTech.Domain.Entities.UserRelated;
using CourseTech.Domain.Result;

namespace CourseTech.Domain.Interfaces.Validators;

/// <summary>
/// Валидатор для сервиса аутентификации.
/// </summary>
public interface IAuthValidator
{
    /// <summary>
    /// Валидация логина пользователя
    /// </summary>
    /// <param name="user"></param>
    /// <param name="enteredPassword"></param>
    /// <returns></returns>
    BaseResult ValidateLogin(User user, string enteredPassword);

    /// <summary>
    /// Валидация регистрации пользователя
    /// </summary>
    /// <param name="user"></param>
    /// <param name="enteredPassword"></param>
    /// <param name="enteredPasswordConfirm"></param>
    /// <returns></returns>
    BaseResult ValidateRegister(User user, string enteredPassword, string enteredPasswordConfirm);
}
