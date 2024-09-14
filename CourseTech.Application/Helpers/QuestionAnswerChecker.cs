using Azure.Core;
using CourseTech.Domain.Constants;
using CourseTech.Domain.Dto.Question.CheckQuestions;
using CourseTech.Domain.Dto.Question.Pass;
using CourseTech.Domain.Dto.Question.QuestionUserAnswer;
using CourseTech.Domain.Dto.TestVariant;
using CourseTech.Domain.Helpers;
using CourseTech.Domain.Interfaces.Dtos.Question;
using CourseTech.Domain.Interfaces.Graph;
using CourseTech.Domain.Interfaces.Helpers;
using CourseTech.Domain.Parameters;

namespace CourseTech.Application.Helpers
{
    public class QuestionAnswerChecker(IQueryGraphAnalyzer queryGraphAnalyzer) : IQuestionAnswerChecker
    {
        public List<ICorrectAnswerDto> CheckUserAnswers(List<ICheckQuestionDto> checkQuestionDtos, List<IUserAnswerDto> userAnswers, out float userGrade)
        {
            var correctAnswers = new List<ICorrectAnswerDto>();
            userGrade = 0;

            for (int i = 0; i < userAnswers.Count; i++)
            {
                var userAnswer = userAnswers[i];
                var checkQuestionDto = checkQuestionDtos[i];

                ICorrectAnswerDto correctAnswer;

                switch (userAnswer)
                {
                    case TestQuestionUserAnswerDto testUserAnswer:
                        correctAnswer = CheckTestQuestionAnswer(testUserAnswer, (checkQuestionDto as TestQuestionCheckingDto).CorrectVariant, ref userGrade);
                        break;
                    case OpenQuestionUserAnswerDto openQuestionUserAnswerDto:
                        correctAnswer = CheckOpenQuestionAnswer(openQuestionUserAnswerDto, (checkQuestionDto as OpenQuestionCheckingDto).OpenQuestionsAnswers, ref userGrade);
                        break;
                    case PracticalQuestionUserAnswerDto practicalQuestionUserAnswerDto:
                        correctAnswer = CheckPracticalQuestionAnswer(practicalQuestionUserAnswerDto, checkQuestionDto as PracticalQuestionCheckingDto, out float questionGrade);
                        userGrade += questionGrade;
                        break;
                    default:
                        return new List<ICorrectAnswerDto>();
                }

                correctAnswers.Add(correctAnswer);
            }

            return correctAnswers;
        }

        private ICorrectAnswerDto CheckTestQuestionAnswer(TestQuestionUserAnswerDto userAnswer, TestVariantDto correctTestVariant, ref float userGrade)
        {
            var correctAnswer = new TestQuestionCorrectAnswerDto
            {
                Id = userAnswer.QuestionId,
                DisplayAnswer = correctTestVariant.DisplayAnswer,
                AnswerCorrectness = userAnswer.UserAnswerNumberOfVariant == correctTestVariant.Number
            };

            if (correctAnswer.AnswerCorrectness)
            {
                userGrade += QuestionGradeConstants.TestQuestionGrade;
            }

            return correctAnswer;
        }

        private ICorrectAnswerDto CheckPracticalQuestionAnswer(PracticalQuestionUserAnswerDto userAnswer, PracticalQuestionCheckingDto questionChecking, out float questionGrade)
        {
            questionGrade = 0;

            var correctAnswer = new PracticalQuestionCorrectAnswerDto
            {
                Id = userAnswer.QuestionId,
                CorrectAnswer = questionChecking.CorrectQueryCode,
                AnswerCorrectness = false
            };

            try
            {
                var userResult = SqlHelper.ExecuteQuery(userAnswer.UserCodeAnswer);
                var rightResult = SqlHelper.ExecuteQuery(questionChecking.CorrectQueryCode);

                DataTableComparer comparer = new DataTableComparer();
                int result = comparer.Compare(userResult, rightResult);

                if (result == 0)
                {
                    correctAnswer.AnswerCorrectness = true;
                    questionGrade += QuestionGradeConstants.PracticalQuestionGrade;
                    correctAnswer.QueryResult = userResult;
                }
                else
                {
                    queryGraphAnalyzer.CalculateUserQueryScore(userAnswer.UserCodeAnswer.ToLower(),
                        questionChecking.PracticalQuestionKeywords,
                        out float practicalQuestionGrade,
                        out List<string> remarks);
                        
                    correctAnswer.Remarks = remarks;
                    correctAnswer.QuestionUserGrade = practicalQuestionGrade;
                    questionGrade += practicalQuestionGrade;
                }
            }
            catch (Exception)
            {
                queryGraphAnalyzer.CalculateUserQueryScore(userAnswer.UserCodeAnswer.ToLower(),
                        questionChecking.PracticalQuestionKeywords,
                        out float practicalQuestionGrade,
                        out List<string> remarks);

                correctAnswer.Remarks = remarks;
                correctAnswer.QuestionUserGrade = practicalQuestionGrade;
            }

            return correctAnswer;
        }

        private ICorrectAnswerDto CheckOpenQuestionAnswer(OpenQuestionUserAnswerDto userAnswer, List<string> openQuestionAnswerVariants, ref float userGrade)
        {
            var correctAnswer = new OpenQuestionCorrectAnswerDto
            {
                Id = userAnswer.QuestionId,
                CorrectAnswer = openQuestionAnswerVariants.FirstOrDefault(),
                AnswerCorrectness = openQuestionAnswerVariants.Any(v => v == userAnswer.UserAnswer)
            };

            if (correctAnswer.AnswerCorrectness)
            {
                userGrade += QuestionGradeConstants.OpenQuestionGrade;
            }

            return correctAnswer;
        }
    }
}
