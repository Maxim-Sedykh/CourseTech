namespace CourseTech.Domain.Settings;

/// <summary>
/// Настройки для передачи их в ChatGPT API
/// </summary>
public class ChatGptSettings
{
    /// <summary>
    /// Модель ChatGpt
    /// </summary>
    public string ChatGptModel { get; set; }

    /// <summary>
    /// Ключ API
    /// </summary>
    public string ApiKey { get; set; }

    /// <summary>
    /// URL на который отправляем запрос
    /// </summary>
    public string BaseUrl { get; set; }

    public string Role { get; set; }

    public int MaxTokens { get; set; }
}
