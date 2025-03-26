namespace CourseTech.Domain.Dto.Auth;

/// <summary>
/// Модель данных для смены пароля пользователя.
/// </summary>
public class ChangePasswordDto
{
    public string NewPassword { get; set; }
}
