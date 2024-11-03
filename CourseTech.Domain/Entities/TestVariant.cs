using CourseTech.Domain.Entities.QuestionEntities.QuestionTypesEntities;
using CourseTech.Domain.Interfaces.Entities;

namespace CourseTech.Domain.Entities;

/// <summary>
/// Вариант ответа на тестовый тип вопроса.
/// </summary>
public class TestVariant : IEntityId<int>, IAuditable
{
    public int Id { get; set; }

    /// <summary>
    /// Идентификатор вопроса тестового типа.
    /// </summary>
    public int TestQuestionId { get; set; }

    /// <summary>
    /// Вопрос тестового типа.
    /// </summary>
    public TestQuestion TestQuestion { get; set; }

    /// <summary>
    /// Номер варианта для ответа на вопрос.
    /// </summary>
    public int VariantNumber { get; set; }

    /// <summary>
    /// Ответ на вопрос, который отображается для пользователя.
    /// </summary>
    public string Content { get; set; }

    /// <summary>
    /// Правильный ли вариант ответа.
    /// </summary>
    public bool IsCorrect { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
