using CourseTech.ChatGptApi.Interfaces;
using CourseTech.Domain.Dto.Analyzer;
using CourseTech.Domain.Interfaces.UserQueryAnalyzers;
using System.Text.Json;

namespace CourseTech.DAL.UserQueryAnalyzers
{
    public class ChatGptQueryAnalyzer(IChatGptService chatGptService) : IChatGptQueryAnalyzer
    {
        public async Task<ChatGptAnalysResponseDto> AnalyzeUserQuery(string userQueryExceptionMessage,
            string userQuery,
            string rightQuery,
            float maxGradeForQuestion)
        {
            var chatGptRequest = $@"Проанализируй запрос ученика. Он делает запросы к тестовой базе данных по теме Кинотеатр.
                Его запрос выдал следующее сообщение в системе {userQueryExceptionMessage}. Оповести его об этом сообщении.
                Есть правильный запрос {rightQuery}, Запрос пользователя {userQuery} не дал тех же результатов что правильный запрос.
                Проанализируй почему. Выдай анализ в виде замечаний ученику. Используй алгоритм графа принятия решений в своём анализе.
                За правильный ответ ученик мог бы получить {maxGradeForQuestion} баллов. Какую часть балла можно ученику дать за его запрос?
                Дай ответ ученику. Отдай ответ в виде JSON. в котором есть свойства UserQueryAnalys в которое запиши свой анализ,
                и свойство UserQueryGrade в котором запиши предполагаемую оценку пользователю за его запрос. Отдавай ответ только в виде валидного JSON,
                без лишних слов. Чтобы его можно было спарсить в соответствующую модель. Сгенерируй JSON объект, который содержит следующие поля:
                - ""UserQueryAnalys"" (строка): Содержит анализ запроса пользователя.
                - ""UserQueryGrade"" (число с плавающей точкой): Содержит оценку запроса пользователя.

                Пример JSON:
                {{
                 ""UserQueryAnalys"": ""Текст анализа запроса"",
                 ""UserQueryGrade"": 1.0
                }}

                Пожалуйста, очень прошу, не добавляй никаких дополнительных символов или разметки, таких как тройные кавычки или обратные апострофы.
                Сделай так чтобы твой ответ можно было спарсить в json модельку без ошибок.";

            var chatGptResponseJson = await chatGptService.SendMessageToChatGPT(chatGptRequest);

            var result = JsonSerializer.Deserialize<ChatGptAnalysResponseDto>(chatGptResponseJson);

            return result;
        }
    }
}
