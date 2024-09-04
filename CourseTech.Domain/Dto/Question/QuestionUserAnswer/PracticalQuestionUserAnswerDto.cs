using CourseTech.Domain.Interfaces.Dtos.Question;

namespace CourseTech.Domain.Dto.Question.QuestionUserAnswer
{
    public class PracticalQuestionUserAnswerDto : IUserAnswerDto
    {
        public int QuestionId { get; set; }

        public string UserCodeAnswer { get; set; }
    }
}
