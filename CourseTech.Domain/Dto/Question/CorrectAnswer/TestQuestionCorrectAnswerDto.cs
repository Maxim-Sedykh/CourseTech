using CourseTech.Domain.Enum;
using CourseTech.Domain.Interfaces.Dtos.Question;
using System.Data;

namespace CourseTech.Domain.Dto.Question.Pass
{
    public class TestQuestionCorrectAnswerDto : ICorrectAnswerDto
    {
        public int Id { get; set; }

        public string CorrectAnswer { get; set; }

        public bool AnswerCorrectness { get; set; }
    }
}
