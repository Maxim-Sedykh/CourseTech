using CourseTech.Domain.Entities.UserRelated;

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
    Result ValidateLogin(User user, string enteredPassword);

    /// <summary>
    /// Валидация регистрации пользователя
    /// </summary>
    /// <param name="user"></param>
    /// <param name="enteredPassword"></param>
    /// <param name="enteredPasswordConfirm"></param>
    /// <returns></returns>
    Result ValidateRegister(User user, string enteredPassword, string enteredPasswordConfirm);
}
