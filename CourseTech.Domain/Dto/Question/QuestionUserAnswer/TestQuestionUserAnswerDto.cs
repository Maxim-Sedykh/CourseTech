using CourseTech.Domain.Interfaces.Dtos.Question;

namespace CourseTech.Domain.Dto.Question.QuestionUserAnswer
{
    /// <summary>
    /// Модель данных для ответа пользователя на вопрос тестового типа.
    /// </summary>
    public class TestQuestionUserAnswerDto : IUserAnswerDto
    {
        public int QuestionId { get; set; }

        public byte UserAnswerNumberOfVariant { get; set; }

        public string QuestionType { get; set; } = "TestQuestionUserAnswerDto";
    }
}
