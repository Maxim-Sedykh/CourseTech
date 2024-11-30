using CourseTech.Application.Resources;
using CourseTech.DAL.Views;
using CourseTech.Domain.Comparers;
using CourseTech.Domain.Dto.Question;
using CourseTech.Domain.Dto.Question.CheckQuestions;
using CourseTech.Domain.Dto.Question.Pass;
using CourseTech.Domain.Dto.Question.QuestionUserAnswer;
using CourseTech.Domain.Dto.TestVariant;
using CourseTech.Domain.Entities.QuestionEntities.QuestionTypesEntities;
using CourseTech.Domain.Interfaces.Databases;
using CourseTech.Domain.Interfaces.Dtos.Question;
using CourseTech.Domain.Interfaces.Graph;
using CourseTech.Domain.Interfaces.Helpers;
using System.Text.RegularExpressions;

namespace CourseTech.Application.Helpers
{
    public class QuestionAnswerChecker(IQueryGraphAnalyzer queryGraphAnalyzer, ISqlQueryProvider sqlProvider) : IQuestionAnswerChecker
    {
        /// <summary>
        /// Оценка за тестовое задание
        /// </summary>
        private float _testQuestionGrade;

        /// <summary>
        /// Оценка за задание открытого типа
        /// </summary>
        private float _openQuestionGrade;

        /// <summary>
        /// Оценка за задание практического типа
        /// </summary>
        private float _practicalQuestionGrade;

        ///<inheritdoc/> 
        public async Task<List<ICorrectAnswerDto>> CheckUserAnswers(List<ICheckQuestionDto> checkQuestionDtos,
            List<IUserAnswerDto> userAnswers,
            UserGradeDto userGrade,
            List<QuestionTypeGrade> questionTypeGrade)
        {
            var questionTypeGrades = questionTypeGrade.ToDictionary(x => x.QuestionTypeName, x => x.Grade);

            _testQuestionGrade = questionTypeGrades.GetValueOrDefault(nameof(TestQuestion), default);
            _openQuestionGrade = questionTypeGrades.GetValueOrDefault(nameof(OpenQuestion), default);
            _practicalQuestionGrade = questionTypeGrades.GetValueOrDefault(nameof(PracticalQuestion), default);

            var correctAnswers = new List<ICorrectAnswerDto>();
            userGrade.Grade = 0;

            for (int i = 0; i < userAnswers.Count; i++)
            {
                var userAnswer = userAnswers[i];
                var checkQuestionDto = checkQuestionDtos[i];

                if (userAnswer.QuestionId != checkQuestionDto.QuestionId)
                {
                    return new List<ICorrectAnswerDto>();
                }

                ICorrectAnswerDto correctAnswer = await CheckAnswer(userAnswer, checkQuestionDto, userGrade);
                correctAnswers.Add(correctAnswer);
            }

            return correctAnswers;
        }

        /// <summary>
        /// Проверить на правильность ответ пользователя, и получить за него оценку.
        /// </summary>
        /// <param name="userAnswer"></param>
        /// <param name="checkQuestionDto"></param>
        /// <param name="userGrade"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        private async Task<ICorrectAnswerDto> CheckAnswer(IUserAnswerDto userAnswer, ICheckQuestionDto checkQuestionDto, UserGradeDto userGrade)
        {
            return userAnswer switch
            {
                TestQuestionUserAnswerDto testUserAnswer =>
                    CheckTestQuestionAnswer(testUserAnswer, (checkQuestionDto as TestQuestionCheckingDto).CorrectVariant, userGrade),

                OpenQuestionUserAnswerDto openUserAnswer =>
                    CheckOpenQuestionAnswer(openUserAnswer, (checkQuestionDto as OpenQuestionCheckingDto).OpenQuestionsAnswers, userGrade),

                PracticalQuestionUserAnswerDto practicalUserAnswer =>
                    await CheckPracticalQuestionAnswer(practicalUserAnswer, checkQuestionDto as PracticalQuestionCheckingDto, userGrade),

                _ => throw new ArgumentException(ErrorMessage.InvalidQuestionType)
            };
        }

        /// <summary>
        /// Проверить ответ на вопрос тестового типа.
        /// </summary>
        /// <param name="userAnswer"></param>
        /// <param name="correctTestVariant"></param>
        /// <param name="userGrade"></param>
        /// <returns></returns>
        private ICorrectAnswerDto CheckTestQuestionAnswer(TestQuestionUserAnswerDto userAnswer, TestVariantDto correctTestVariant, UserGradeDto userGrade)
        {
            bool isCorrect = userAnswer.UserAnswerNumberOfVariant == correctTestVariant.VariantNumber;

            if (isCorrect)
            {
                userGrade.Grade += _testQuestionGrade;
            }

            return new TestQuestionCorrectAnswerDto
            {
                Id = userAnswer.QuestionId,
                CorrectAnswer = correctTestVariant.Content,
                AnswerCorrectness = isCorrect
            };
        }

        /// <summary>
        /// Проверить ответ на вопрос открытого типа.
        /// </summary>
        /// <param name="userAnswer"></param>
        /// <param name="openQuestionAnswerVariants"></param>
        /// <param name="userGrade"></param>
        /// <returns></returns>
        private ICorrectAnswerDto CheckOpenQuestionAnswer(OpenQuestionUserAnswerDto userAnswer, List<string> openQuestionAnswerVariants, UserGradeDto userGrade)
        {
            string normalizedUserAnswer = Regex.Replace(userAnswer.UserAnswer.ToLower().Trim(), @"s+", " ");

            var correctAnswer = new OpenQuestionCorrectAnswerDto
            {
                Id = userAnswer.QuestionId,
                CorrectAnswer = openQuestionAnswerVariants.FirstOrDefault(),
                AnswerCorrectness = openQuestionAnswerVariants.Any(v => v == normalizedUserAnswer)
            };

            if (correctAnswer.AnswerCorrectness)
            {
                userGrade.Grade += _openQuestionGrade;
            }

            return correctAnswer;
        }

        /// <summary>
        /// Проверить ответ на вопрос практического типа.
        /// </summary>
        /// <param name="userAnswer"></param>
        /// <param name="questionChecking"></param>
        /// <param name="userGrade"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        private async Task<ICorrectAnswerDto> CheckPracticalQuestionAnswer(PracticalQuestionUserAnswerDto userAnswer, PracticalQuestionCheckingDto questionChecking, UserGradeDto userGrade)
        {
            var correctAnswer = new PracticalQuestionCorrectAnswerDto
            {
                Id = userAnswer.QuestionId,
                CorrectAnswer = questionChecking.CorrectQueryCode,
                AnswerCorrectness = false
            };

            try
            {
                var userResult = await sqlProvider.ExecuteQueryAsync(userAnswer.UserCodeAnswer);
                var correctResult = await sqlProvider.ExecuteQueryAsync(questionChecking.CorrectQueryCode);

                if (DynamicListComparer.AreListsOfDynamicEqual(userResult, correctResult))
                {
                    correctAnswer.AnswerCorrectness = true;
                    correctAnswer.QueryResult = userResult;

                    userGrade.Grade += _practicalQuestionGrade;
                }
                else
                {
                    correctAnswer.QueryResult = userResult;

                    throw new InvalidOperationException(ErrorMessage.InvalidUserQuery);
                }
            }
            catch (Exception ex)
            {
                var remarks = GetRemarks(userAnswer.UserCodeAnswer.ToLower(), questionChecking.PracticalQuestionKeywords, out float practicalQuestionGrade);
                remarks.Insert(0, ex.Message);
                correctAnswer.QuestionUserGrade = practicalQuestionGrade;
                userGrade.Grade += practicalQuestionGrade;
            }

            return correctAnswer;
        }

        /// <summary>
        /// Получить замечания для пользователя, и его оценку за ответ, исходя из его запроса.
        /// </summary>
        /// <param name="userCodeAnswer"></param>
        /// <param name="keywords"></param>
        /// <param name="practicalQuestionGrade"></param>
        /// <returns></returns>
        private List<string> GetRemarks(string userCodeAnswer, List<string> keywords, out float practicalQuestionGrade)
        {
            queryGraphAnalyzer.CalculateUserQueryScore(userCodeAnswer, keywords, out practicalQuestionGrade, out List<string> remarks);
            return remarks;
        }
    }
}
