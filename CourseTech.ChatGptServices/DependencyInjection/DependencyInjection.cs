using CourseTech.ChatGptApi.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace CourseTech.ChatGptApi.DependencyInjection;

public static class DependencyInjection
{
    public static void AddChatGpt(this IServiceCollection services)
    {
        services.AddScoped<IChatGptService, ChatGptService>();
    }
}
