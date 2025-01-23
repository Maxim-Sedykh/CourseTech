using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.ChatGptApi.Constants
{
    /// <summary>
    /// Класс для хранения констант - названий свойств в теле ответа из Chat Gpt API
    /// </summary>
    public class ChatGptReponsePropertyConstants
    {
        public const string FirstLevelResponseProperty = "choices";

        public const string SecondLevelResponseProperty = "message";

        public const string ThirdLevelResponseProperty = "content";
    }
}
