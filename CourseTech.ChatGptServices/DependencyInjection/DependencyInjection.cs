using CourseTech.ChatGptApi.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace CourseTech.ChatGptApi.DependencyInjection;

/// <summary>
/// Внедрение зависимостей ChatGPT сервиса
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Зарегистрировать сервис для работы с ChatGPT
    /// </summary>
    /// <param name="services"></param>
    public static void AddChatGpt(this IServiceCollection services)
    {
        services.AddHttpClient<IChatGptService, ChatGptService>(client =>
        {
            client.Timeout = TimeSpan.FromSeconds(50);
        });

        services.AddScoped<IChatGptService, ChatGptService>();
    }
}
