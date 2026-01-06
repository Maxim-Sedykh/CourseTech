namespace CourseTech.ChatGptApi.Models;

/// <summary>
/// Модель для запроса в ChatGPT API
/// </summary>
/// <param name="Model"></param>
/// <param name="Messages"></param>
public record GptRequest(string Model, GptMessage[] Messages);
