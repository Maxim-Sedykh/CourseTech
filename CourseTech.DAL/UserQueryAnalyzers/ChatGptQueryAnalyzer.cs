using CourseTech.ChatGptApi.Interfaces;
using CourseTech.Domain.Dto.Analyzer;
using CourseTech.Domain.Interfaces.UserQueryAnalyzers;
using System.Text.Json;

namespace CourseTech.DAL.UserQueryAnalyzers;

public class ChatGptQueryAnalyzer(IChatGptService chatGptService) : IChatGptQueryAnalyzer
{
    public async Task<ChatGptAnalysResponseDto> AnalyzeUserQuery(
        string userQueryExceptionMessage,
        string userQuery,
        string rightQuery)
    {
        var chatGptRequest = $@"
            Побудь в роли учителя. Проанализируй пожалуйста запрос ученика sql.
            Запрос пользователя, семантически, по ключевым словам, раздели на граф принятия решения. Все данные пиши на русском. Анализ SQL-запроса ученика:
            1. Ошибка: {userQueryExceptionMessage}
            2. Запрос ученика: {userQuery}
            3. Правильный запрос: {rightQuery}

            Сформируй ответ СТРОГО в следующем JSON-формате:
            {{
                ""UserQueryAnalys"": ""Твой текстовый анализ ошибки"",
                ""Vertexes"": [
                    {{""Number"": 1, ""Name"": ""Название вершины 1""}},
                    {{""Number"": 2, ""Name"": ""Название вершины 2""}}
                ],
                ""Edges"": [
                    {{""From"": 1, ""To"": 2}}
                ]
            }}

            Требования:
            - Только JSON, без дополнительного текста
            - Все поля обязательные
            - Имена полей в точности как указано
            - Числовые значения без кавычек
            - Не использовать markdown или код-блоки
            - Не добавлять комментарии

            Алгоритм анализа:
            1. Сначала определи основные этапы анализа как вершины графа
            2. Затем определи связи между ними как рёбра
            3. Вершины должны отражать ключевые шаги проверки запроса
            4. Рёбра показывают логическую последовательность шагов";

        try
        {
            var chatGptResponseJson = await chatGptService.SendMessageToChatGPT(chatGptRequest);

            // Очистка ответа
            chatGptResponseJson = CleanJsonResponse(chatGptResponseJson);

            return JsonSerializer.Deserialize<ChatGptAnalysResponseDto>(chatGptResponseJson);
        }
        catch
        {
            // Fallback-решение
            return new ChatGptAnalysResponseDto
            {
                UserQueryAnalys = "Не удалось проанализировать запрос",
                Vertexes = [],
                Edges = []
            };
        }
    }

    private string CleanJsonResponse(string rawResponse)
    {
        // Удаление всех не-JSON элементов
        var jsonStart = rawResponse.IndexOf('{');
        var jsonEnd = rawResponse.LastIndexOf('}') + 1;

        if (jsonStart >= 0 && jsonEnd > jsonStart)
        {
            return rawResponse[jsonStart..jsonEnd]
                .Replace("```json", "")
                .Replace("```", "")
                .Trim();
        }

        return rawResponse;
    }
}
