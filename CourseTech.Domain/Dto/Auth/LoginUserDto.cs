namespace CourseTech.Domain.Dto.Auth;

/// <summary>
/// Модель данных для авторизации пользователя.
/// </summary>
public class LoginUserDto
{
    public string Login { get; set; }
    public string Password { get; set; }
    public bool RememberMe { get; set; }
}
