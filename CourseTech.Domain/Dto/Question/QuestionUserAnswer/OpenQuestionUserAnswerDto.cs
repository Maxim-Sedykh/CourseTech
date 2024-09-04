using CourseTech.Domain.Interfaces.Dtos.Question;

namespace CourseTech.Domain.Dto.Question.QuestionUserAnswer
{
    public class OpenQuestionUserAnswerDto : IUserAnswerDto
    {
        public int QuestionId { get; set; }

        public string UserAnswer { get; set; }
    }
}
