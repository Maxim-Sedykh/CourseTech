using CourseTech.Domain.Interfaces.Dtos.Question;

namespace CourseTech.Domain.Interfaces.Helpers
{
    public interface IQuestionAnswerChecker
    {
        List<ICorrectAnswerDto> CheckUserAnswers(List<ICheckQuestionDto> checkQuestions, List<IUserAnswerDto> userAnswers, out float userGrade);
    }
}
