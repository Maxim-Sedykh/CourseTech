using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.ChatGptApi.Interfaces
{
    public interface IChatGptService
    {
        Task<string> SendMessageToChatGPT(string prompt);
    }
}
