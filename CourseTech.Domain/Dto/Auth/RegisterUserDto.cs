namespace CourseTech.Domain.Dto.Auth;

/// <summary>
/// Модель данных для регистрации пользователя.
/// </summary>
public record RegisterUserDto
{
    public string Login { get; set; }

    public string FirstName { get; set; }
    public string LastName { get; set; }

    public DateTime DateOfBirth { get; set; }

    public string Password { get; set; }

    public string PasswordConfirm { get; set; }
}