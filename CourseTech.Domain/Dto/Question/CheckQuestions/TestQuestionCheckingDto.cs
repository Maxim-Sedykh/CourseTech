using CourseTech.Domain.Dto.TestVariant;
using CourseTech.Domain.Interfaces.Dtos.Question;

namespace CourseTech.Domain.Dto.Question.CheckQuestions
{
    public class TestQuestionCheckingDto : ICheckQuestionDto
    {
        public TestVariantDto CorrectVariant { get; set; }
    }
}
