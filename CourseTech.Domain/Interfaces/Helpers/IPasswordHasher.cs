namespace CourseTech.Domain.Interfaces.Helpers;

/// <summary>
/// Сервис для работы с хэшированием пароля.
/// </summary>
public interface IPasswordHasher
{
    /// <summary>
    /// Захэшировать пароль для БД.
    /// </summary>
    /// <param name="password">Пароль</param>
    /// <returns></returns>
    string Hash(string password);

    /// <summary>
    /// Проверяет, корректный ли пароль ввёл пользователь для авторизации.
    /// </summary>
    /// <param name="enteredPassword">Введённый пользователем пароль.</param>
    /// <param name="passwordHash">Хэш его пароля из БД.</param>
    /// <returns></returns>
    bool Verify(string enteredPassword, string passwordHash);
}
