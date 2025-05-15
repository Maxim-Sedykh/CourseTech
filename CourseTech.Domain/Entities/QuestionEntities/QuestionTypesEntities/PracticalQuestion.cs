namespace CourseTech.Domain.Entities.QuestionEntities.QuestionTypesEntities;

/// <summary>
/// Вопрос практического типа.
/// </summary>
public class PracticalQuestion : BaseQuestion
{
    /// <summary>
    /// Правильный запрос. Ответ на практический вопрос.
    /// Один из вариантов ответа, который даёт правильный результат.
    /// </summary>
    public string CorrectQueryCode { get; set; }

    ///// <summary>
    ///// Служебное слово, которое пользователь обязательно должен использовать,
    ///// чтобы пройти практический вопрос
    ///// </summary>
    //public string StopKeyword { get; set; }
}
