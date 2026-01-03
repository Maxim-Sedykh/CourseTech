using CourseTech.Domain.Dto.Question;
using CourseTech.Domain.Dto.Question.CheckQuestions;
using CourseTech.Domain.Dto.Question.Pass;
using CourseTech.Domain.Dto.Question.QuestionUserAnswer;
using CourseTech.Domain.Interfaces.Dtos.Question;
using CourseTech.Domain.Interfaces.Services.Question;

namespace CourseTech.Application.Services.Question.Strategies
{
    public class TestAnswerCheckingStrategy : IAnswerCheckingStrategy
    {
        public Type UserAnswerType { get; } = typeof(TestQuestionUserAnswerDto);

        public Task<CorrectAnswerDtoBase> CheckAnswerAsync(UserAnswerDtoBase userAnswer, CheckQuestionDtoBase checkQuestion, UserGradeDto userGrade, float questionGrade)
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

            return Task.FromResult((CorrectAnswerDtoBase)result);
        }
    }
}
