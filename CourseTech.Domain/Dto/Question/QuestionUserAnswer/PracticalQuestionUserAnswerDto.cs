using CourseTech.Domain.Interfaces.Dtos.Question;

namespace CourseTech.Domain.Dto.Question.QuestionUserAnswer
{
    /// <summary>
    /// Модель данных для ответа пользователя на вопрос практического типа.
    /// </summary>
    public class PracticalQuestionUserAnswerDto : IUserAnswerDto
    {
        public int QuestionId { get; set; }

        public string UserCodeAnswer { get; set; }

        public string QuestionType { get; set; } = "PracticalQuestionUserAnswerDto";
    }
}
