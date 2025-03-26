namespace CourseTech.Domain.Constants.LearningProcess;

/// <summary>
/// Константы, для того чтобы выдать пользователю замечания, по итогу его прохождения практического задания.
/// </summary>
public static class QueryAnalyzeRemarks
{
    /// <summary>
    /// Символы для разделения слов в SQL-запросе пользователя.
    /// </summary>
    public static readonly char[] SqlQuerySplitters = [' ', ',', '.', '(', ')', ';'];

    /// <summary>
    /// Если пользователь написал правильные слова в запросе, но не в правильном порядке.
    /// </summary>
    public const string KeywordsIncorrectOrderRemark = "Служебные слова расположены не в правильном порядке";

    /// <summary>
    /// Если пользователь пропустил какое-то слово в запросе
    /// </summary>
    public const string MissingKeywordRemark = "Вы не добавили в свой запрос служебное слово {0}";
}
