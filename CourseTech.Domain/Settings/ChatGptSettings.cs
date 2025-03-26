namespace CourseTech.Domain.Settings;

public class ChatGptSettings
{
    public string ChatGptModel { get; set; }

    public string ApiKey { get; set; }

    public string BaseUrl { get; set; }

    public string Role { get; set; }

    public int MaxTokens { get; set; }
}
