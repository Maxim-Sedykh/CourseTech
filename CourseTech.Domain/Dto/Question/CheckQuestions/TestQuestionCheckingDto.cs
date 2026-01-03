using CourseTech.Domain.Dto.TestVariant;
using CourseTech.Domain.Interfaces.Dtos.Question;

namespace CourseTech.Domain.Dto.Question.CheckQuestions;

/// <summary>
/// Модель данных для хранения данных, которые нужны для проверки правильности вопроса тестового типа.
/// </summary>
public class TestQuestionCheckingDto : CheckQuestionDtoBase
{
    public int QuestionId { get; set; }

    public TestVariantDto CorrectVariant { get; set; }
}
