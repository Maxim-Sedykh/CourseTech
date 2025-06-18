using CourseTech.Domain.Dto.Question;
using CourseTech.Domain.Interfaces.Dtos.Question;

namespace CourseTech.Domain.Interfaces.Strategy
{
    public interface IAnswerCheckingStrategy
    {
        Task<ICorrectAnswerDto> CheckAnswerAsync(IUserAnswerDto userAnswer, ICheckQuestionDto checkQuestion, UserGradeDto userGrade, float questionGrade);
    }
}
