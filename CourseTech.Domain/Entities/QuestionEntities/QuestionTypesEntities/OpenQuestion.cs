namespace CourseTech.Domain.Entities.QuestionEntities.QuestionTypesEntities;

/// <summary>
/// Вопрос открытого типа.
/// </summary>
public class OpenQuestion : BaseQuestion
{
    /// <summary>
    /// Возможные варианты ответа на вопрос.
    /// </summary>
    public List<OpenQuestionAnswer> AnswerVariants { get; set; }
}
