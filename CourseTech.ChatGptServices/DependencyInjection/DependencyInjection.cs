using CourseTech.ChatGptApi.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.ChatGptApi.DependencyInjection
{
    public static class DependencyInjection
    {
        public static void AddChatGpt(this IServiceCollection services)
        {
            services.AddScoped<IChatGptService, ChatGptService>();
        }
    }
}
