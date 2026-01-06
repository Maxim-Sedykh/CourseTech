namespace CourseTech.ChatGptApi.Models;

/// <summary>
/// Сообщение в запросе для ChatGPT
/// </summary>
/// <param name="Role"></param>
/// <param name="Content"></param>
public record GptMessage(string Role, string Content);
