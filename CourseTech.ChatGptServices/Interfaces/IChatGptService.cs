namespace CourseTech.ChatGptApi.Interfaces;

public interface IChatGptService
{
    Task<string> SendMessageToChatGPT(string prompt);
}
