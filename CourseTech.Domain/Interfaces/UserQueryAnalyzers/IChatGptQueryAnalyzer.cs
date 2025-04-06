using CourseTech.Domain.Dto.Analyzer;

namespace CourseTech.Domain.Interfaces.UserQueryAnalyzers;

/// <summary>
/// Сервис для анализа запроса пользователей на основе Chat GPT
/// </summary>
public interface IChatGptQueryAnalyzer
{
    /// <summary>
    /// Анализирование запроса пользователя чатом Gpt
    /// </summary>
    /// <param name="userQuery"></param>
    /// <param name="rightQuery"></param>
    /// <returns></returns>
    Task<ChatGptAnalysResponseDto> AnalyzeUserQuery(
        string exceptionMessage,
        string userQuery,
        string rightQuery);
}
