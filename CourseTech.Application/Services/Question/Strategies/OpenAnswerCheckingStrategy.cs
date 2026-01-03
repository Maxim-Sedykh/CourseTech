using CourseTech.Domain.Dto.Question;
using CourseTech.Domain.Dto.Question.CheckQuestions;
using CourseTech.Domain.Dto.Question.Pass;
using CourseTech.Domain.Dto.Question.QuestionUserAnswer;
using CourseTech.Domain.Interfaces.Dtos.Question;
using CourseTech.Domain.Interfaces.Services.Question;

namespace CourseTech.Application.Services.Question.Strategies
{
    public class OpenAnswerCheckingStrategy : IAnswerCheckingStrategy
    {
        public Type UserAnswerType { get; } = typeof(OpenQuestionUserAnswerDto);

        public Task<CorrectAnswerDtoBase> CheckAnswerAsync(UserAnswerDtoBase userAnswer, CheckQuestionDtoBase checkQuestion, UserGradeDto userGrade, float questionGrade)
        {
            var openUserAnswer = (OpenQuestionUserAnswerDto)userAnswer;
            var openQuestionAnswerVariants = ((OpenQuestionCheckingDto)checkQuestion).OpenQuestionsAnswers;

            string normalizedUserAnswer = openUserAnswer.UserAnswer.ToLower().Trim();

            var result = new OpenQuestionCorrectAnswerDto
            {
                Id = openUserAnswer.QuestionId,
                CorrectAnswer = openQuestionAnswerVariants.FirstOrDefault(),
                AnswerCorrectness = openQuestionAnswerVariants.Any(v => v.Equals(normalizedUserAnswer, StringComparison.OrdinalIgnoreCase))
            };

            if (result.AnswerCorrectness)
            {
                userGrade.Grade += questionGrade;
            }

            return Task.FromResult((CorrectAnswerDtoBase)result);
        }
    }
}
