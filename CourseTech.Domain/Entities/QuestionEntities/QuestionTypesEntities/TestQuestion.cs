namespace CourseTech.Domain.Entities.QuestionEntities.QuestionTypesEntities;

/// <summary>
/// Вопрос тестового типа.
/// </summary>
public class TestQuestion : BaseQuestion
{
    /// <summary>
    /// Варианты ответа на тестовый вопрос.
    /// </summary>
    public List<TestVariant> TestVariants { get; set; }
}
