using CourseTech.Domain.Constants.LearningProcess;
using CourseTech.Domain.Dto.Question;
using CourseTech.Domain.Dto.Question.CheckQuestions;
using CourseTech.Domain.Dto.Question.Pass;
using CourseTech.Domain.Dto.Question.QuestionUserAnswer;
using CourseTech.Domain.Dto.TestVariant;
using CourseTech.Domain.Helpers;
using CourseTech.Domain.Interfaces.Databases;
using CourseTech.Domain.Interfaces.Dtos.Question;
using CourseTech.Domain.Interfaces.Graph;
using CourseTech.Domain.Interfaces.Helpers;
using System.Data;
using System.Text.RegularExpressions;

namespace CourseTech.Application.Helpers
{
    public class QuestionAnswerChecker(IQueryGraphAnalyzer queryGraphAnalyzer, ISqlQueryProvider sqlProvider) : IQuestionAnswerChecker
    {

        public async Task<List<ICorrectAnswerDto>> CheckUserAnswers(List<ICheckQuestionDto> checkQuestionDtos, List<IUserAnswerDto> userAnswers, UserGradeDto userGrade)
        {
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

                _ => throw new ArgumentException("Неизвестный тип ответа")
            };
        }

        private ICorrectAnswerDto CheckTestQuestionAnswer(TestQuestionUserAnswerDto userAnswer, TestVariantDto correctTestVariant, UserGradeDto userGrade)
        {
            bool isCorrect = userAnswer.UserAnswerNumberOfVariant == correctTestVariant.VariantNumber;

            if (isCorrect)
            {
                userGrade.Grade += QuestionGrades.TestQuestionGrade;
            }

            return new TestQuestionCorrectAnswerDto
            {
                Id = userAnswer.QuestionId,
                CorrectAnswer = correctTestVariant.Content,
                AnswerCorrectness = isCorrect
            };
        }

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
                userGrade.Grade += QuestionGrades.OpenQuestionGrade;
            }

            return correctAnswer;
        }

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

                if (IsResultsEqual(userResult, correctResult))
                {
                    correctAnswer.AnswerCorrectness = true;
                    correctAnswer.QueryResult = userResult;

                    userGrade.Grade += QuestionGrades.PracticalQuestionGrade;
                }
                else
                {
                    correctAnswer.QueryResult = userResult;

                    throw new InvalidOperationException("Ваш ответ не совпадает с правильным ответом.");
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

        private bool IsResultsEqual(DataTable userResult, DataTable correctResult)
        {
            var comparer = new DataTableComparer();
            return comparer.Compare(userResult, correctResult) == 0;
        }

        private List<string> GetRemarks(string userCodeAnswer, List<string> keywords, out float practicalQuestionGrade)
        {
            queryGraphAnalyzer.CalculateUserQueryScore(userCodeAnswer, keywords, out practicalQuestionGrade, out List<string> remarks);
            return remarks;
        }
    }
}
