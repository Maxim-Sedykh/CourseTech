namespace CourseTech.ChatGptApi.Models;

/// <summary>
/// Ответ от ChatGPT API
/// </summary>
/// <param name="Choices"></param>
public record GptResponse(GptChoice[] Choices);
