using CourseTech.Domain.Dto.TestVariant;
using CourseTech.Domain.Interfaces.Dtos.Question;

namespace CourseTech.Domain.Dto.Question.GetQuestions;

/// <summary>
/// Модель данных для отображения вопроса тестового типа.
/// </summary>
public class TestQuestionDto : QuestionDtoBase
{
    public List<TestVariantDto> TestVariants { get; set; }
}
