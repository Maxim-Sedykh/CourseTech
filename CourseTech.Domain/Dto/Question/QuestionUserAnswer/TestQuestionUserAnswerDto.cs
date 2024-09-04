using CourseTech.Domain.Interfaces.Dtos.Question;

namespace CourseTech.Domain.Dto.Question.QuestionUserAnswer
{
    public class TestQuestionUserAnswerDto : IUserAnswerDto
    {
        public int QuestionId { get; set; }

        public byte UserAnswerNumberOfVariant { get; set; }
    }
}
