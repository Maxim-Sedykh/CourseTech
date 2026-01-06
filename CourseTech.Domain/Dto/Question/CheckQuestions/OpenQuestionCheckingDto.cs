using CourseTech.Domain.Interfaces.Dtos.Question;

namespace CourseTech.Domain.Dto.Question.CheckQuestions;

/// <summary>
/// Модель данных для хранения данных, которые нужны для проверки правильности вопроса открытого типа.
/// </summary>
public class OpenQuestionCheckingDto : CheckQuestionDtoBase
{
    public List<string> OpenQuestionsAnswers { get; set; }
}
