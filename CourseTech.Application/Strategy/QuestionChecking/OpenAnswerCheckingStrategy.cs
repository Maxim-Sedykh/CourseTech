using CourseTech.Domain.Dto.Question;
using CourseTech.Domain.Dto.Question.CheckQuestions;
using CourseTech.Domain.Dto.Question.Pass;
using CourseTech.Domain.Dto.Question.QuestionUserAnswer;
using CourseTech.Domain.Interfaces.Dtos.Question;
using CourseTech.Domain.Interfaces.Strategy;

namespace CourseTech.Application.Strategy.QuestionChecking
{
    public class OpenAnswerCheckingStrategy : IAnswerCheckingStrategy
    {
        public Task<ICorrectAnswerDto> CheckAnswerAsync(IUserAnswerDto userAnswer, ICheckQuestionDto checkQuestion, UserGradeDto userGrade, float questionGrade)
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

            return Task.FromResult((ICorrectAnswerDto)result);
        }
    }
}
