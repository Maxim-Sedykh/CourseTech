using CourseTech.Domain.Dto.Question;
using CourseTech.Domain.Dto.Question.CheckQuestions;
using CourseTech.Domain.Dto.Question.Pass;
using CourseTech.Domain.Dto.Question.QuestionUserAnswer;
using CourseTech.Domain.Interfaces.Dtos.Question;
using CourseTech.Domain.Interfaces.Strategy;

namespace CourseTech.Application.Strategy.QuestionChecking
{
    public class TestAnswerCheckingStrategy : IAnswerCheckingStrategy
    {
        public Task<ICorrectAnswerDto> CheckAnswerAsync(IUserAnswerDto userAnswer, ICheckQuestionDto checkQuestion, UserGradeDto userGrade, float questionGrade)
        {
            var testUserAnswer = (TestQuestionUserAnswerDto)userAnswer;
            var correctTestVariant = ((TestQuestionCheckingDto)checkQuestion).CorrectVariant;

            bool isCorrect = testUserAnswer.UserAnswerNumberOfVariant == correctTestVariant.VariantNumber;

            if (isCorrect)
            {
                userGrade.Grade += questionGrade;
            }

            var result = new TestQuestionCorrectAnswerDto
            {
                Id = testUserAnswer.QuestionId,
                CorrectAnswer = correctTestVariant.Content,
                AnswerCorrectness = isCorrect
            };

            return Task.FromResult((ICorrectAnswerDto)result);
        }
    }
}
