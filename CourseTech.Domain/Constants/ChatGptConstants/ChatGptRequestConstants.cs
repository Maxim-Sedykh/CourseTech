using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Domain.Constants.ChatGptConstants
{
    public class ChatGptRequestConstants
    {
        public const string ChatGptRequest = @"Проанализируй запрос ученика. Он делает запросы к тестовой базе данных по теме Кинотеатр.
                Его запрос выдал следующее сообщение в системе {userQueryExceptionMessage}. Оповести его об этом сообщении.
                Есть правильный запрос {rightQuery}, Запрос пользователя {userQuery} не дал тех же результатов что правильный запрос.
                Проанализируй почему. Выдай анализ в виде замечаний ученику. Используй алгоритм графа принятия решений в своём анализе.
                За правильный ответ ученик мог бы получить {maxGradeForQuestion} баллов. Какую часть балла можно ученику дать за его запрос?
                Дай ответ ученику. Отдай ответ в виде JSON. в котором есть свойства UserQueryAnalys в которое запиши свой анализ,
                и свойство UserQueryGrade в котором запиши предполагаемую оценку пользователю за его запрос. Отдавай ответ только в виде JSON,
                без лишних слов. Чтобы его можно было спарсить в соответствующую модель.";
    }
}
