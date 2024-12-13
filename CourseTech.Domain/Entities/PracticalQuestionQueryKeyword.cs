using CourseTech.Domain.Entities.QuestionEntities.QuestionTypesEntities;

namespace CourseTech.Domain.Entities;

/// <summary>
/// Ключевые слова корректных запросов для практического типа заданий.
/// Таблица сцепка, для реализации связи "Многие ко многим" между практическими вопросами и ключевыми словами.
/// </summary>
public class PracticalQuestionQueryKeyword
{
    /// <summary>
    /// Номер по порядку ключевого слова в запросе.
    /// </summary>
    public int Number { get; set; }

    /// <summary>
    /// Идентификатор ключевого слова.
    /// </summary>
    public int KeywordId { get; set; }

    /// <summary>
    /// Если данное ключевое слово не указано в запросе пользователя,
    /// то ему автоматически задача не засчитывается
    /// </summary>
    public bool IsStopWord { get; set; }

    /// <summary>
    /// Ключевое слово.
    /// </summary>
    public Keyword Keyword { get; set; }

    /// <summary>
    /// Идентификатор вопроса практического типа.
    /// </summary>
    public int PracticalQuestionId { get; set; }

    /// <summary>
    /// Вопрос практического типа.
    /// </summary>
    public PracticalQuestion PracticalQuestion { get; set; }
}
