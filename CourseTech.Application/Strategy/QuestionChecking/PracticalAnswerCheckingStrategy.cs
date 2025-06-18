using CourseTech.Application.Resources;
using CourseTech.Domain.Comparers;
using CourseTech.Domain.Dto.Question;
using CourseTech.Domain.Dto.Question.CheckQuestions;
using CourseTech.Domain.Dto.Question.CorrectAnswer;
using CourseTech.Domain.Dto.Question.QuestionUserAnswer;
using CourseTech.Domain.Interfaces.Databases;
using CourseTech.Domain.Interfaces.Dtos.Question;
using CourseTech.Domain.Interfaces.Strategy;
using CourseTech.Domain.Interfaces.UserQueryAnalyzers;
using Microsoft.Extensions.Logging;

namespace CourseTech.Application.Strategy.QuestionChecking
{
    public class PracticalAnswerCheckingStrategy(ISqlQueryProvider sqlProvider, IChatGptQueryAnalyzer chatGptAnalyzer, ILogger logger) : IAnswerCheckingStrategy
    {

        public async Task<ICorrectAnswerDto> CheckAnswerAsync(IUserAnswerDto userAnswer, ICheckQuestionDto checkQuestion, UserGradeDto userGrade, float questionGrade)
        {
            var practicalUserAnswer = (PracticalQuestionUserAnswerDto)userAnswer;
            var questionChecking = (PracticalQuestionCheckingDto)checkQuestion;

            var correctAnswer = new PracticalQuestionCorrectAnswerDto
            {
                Id = practicalUserAnswer.QuestionId,
                CorrectAnswer = questionChecking.CorrectQueryCode,
                AnswerCorrectness = false
            };

            (List<dynamic>, double correctQueryTime) correctResult;
            (List<dynamic>, double userQueryTime) userResult;

            try
            {
                correctResult = await sqlProvider.ExecuteQueryAsync(questionChecking.CorrectQueryCode.ToLower());
                correctAnswer.CorrectQueryTime = correctResult.correctQueryTime;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Correct query in test database has fallen with error {ex.Message}");
                throw;
            }

            try
            {
                userResult = await sqlProvider.ExecuteQueryAsync(practicalUserAnswer.UserCodeAnswer.ToLower().Trim());
                correctAnswer.UserQueryTime = userResult.userQueryTime;

                if (DynamicListComparer.AreListsOfDynamicEqual(userResult.Item1, correctResult.Item1))
                {
                    correctAnswer.AnswerCorrectness = true;
                    userGrade.Grade += questionGrade;
                }
                else
                {
                    throw new InvalidOperationException(ErrorMessage.InvalidUserQuery);
                }
            }
            catch (Exception ex)
            {
                var userQueryChatGptAnalysDto = await chatGptAnalyzer.AnalyzeUserQuery(
                    ex.Message,
                    practicalUserAnswer.UserCodeAnswer,
                    questionChecking.CorrectQueryCode,
                    correctAnswer.UserQueryTime,
                    correctAnswer.CorrectQueryTime);

                correctAnswer.QuestionUserGrade = 0;
                correctAnswer.ChatGptAnalysis = userQueryChatGptAnalysDto;
            }

            return correctAnswer;
        }
    }
}
