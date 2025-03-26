namespace CourseTech.Domain.Dto.Token;

/// <summary>
/// Модель данных для отображения токена.
/// </summary>
public class TokenDto
{
    public string AccessToken { get; set; }

    public string RefreshToken { get; set; }
}
