using CourseTech.Domain.Dto.Question;
using CourseTech.Domain.Interfaces.Dtos.Question;

namespace CourseTech.Domain.Interfaces.Services.Question
{
    public interface IAnswerCheckingStrategy
    {
        Type UserAnswerType { get; }

        Task<CorrectAnswerDtoBase> CheckAnswerAsync(UserAnswerDtoBase userAnswer, CheckQuestionDtoBase checkQuestion, UserGradeDto userGrade, float questionGrade);
    }
}
