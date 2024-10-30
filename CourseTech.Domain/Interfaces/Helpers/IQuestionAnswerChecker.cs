using CourseTech.Domain.Dto.Question;
using CourseTech.Domain.Interfaces.Dtos.Question;

namespace CourseTech.Domain.Interfaces.Helpers
{
    public interface IQuestionAnswerChecker
    {
        Task<List<ICorrectAnswerDto>> CheckUserAnswers(List<ICheckQuestionDto> checkQuestions, List<IUserAnswerDto> userAnswers, UserGradeDto userGrade);
    }
}
