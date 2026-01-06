namespace CourseTech.ChatGptApi.Interfaces;

/// <summary>
/// Интерфейс сервиса для работы с ChatGPT
/// </summary>
public interface IChatGptService
{
    /// <summary>
    /// Отправить сообщение в ChatGPT
    /// </summary>
    /// <param name="prompt">строковый промпт</param>
    /// <returns>Ответ в виде строки от ChatGPT</returns>
    Task<string> SendMessageToChatGPT(string prompt);
}
