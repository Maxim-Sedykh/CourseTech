using CourseTech.Domain.Dto.Analyzer;
using CourseTech.Domain.Interfaces.Dtos.Question;

namespace CourseTech.Domain.Dto.Question.CorrectAnswer;

/// <summary>
/// Модель данных для отображения правильного ответа на вопрос практического типа.
/// </summary>
public class PracticalQuestionCorrectAnswerDto : CorrectAnswerDtoBase
{
    public float QuestionUserGrade { get; set; }

    public ChatGptAnalysResponseDto ChatGptAnalysis { get; set; }
}
