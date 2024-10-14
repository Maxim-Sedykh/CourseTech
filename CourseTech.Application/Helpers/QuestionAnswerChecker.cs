using Azure.Core;
using CourseTech.Domain.Constants.LearningProcess;
using CourseTech.Domain.Dto.Question.CheckQuestions;
using CourseTech.Domain.Dto.Question.Pass;
using CourseTech.Domain.Dto.Question.QuestionUserAnswer;
using CourseTech.Domain.Dto.TestVariant;
using CourseTech.Domain.Helpers;
using CourseTech.Domain.Interfaces.Databases;
using CourseTech.Domain.Interfaces.Dtos.Question;
using CourseTech.Domain.Interfaces.Graph;
using CourseTech.Domain.Interfaces.Helpers;
using CourseTech.Domain.Parameters;

namespace CourseTech.Application.Helpers
{
    public class QuestionAnswerChecker(IQueryGraphAnalyzer queryGraphAnalyzer, ISqlHelper sqlHelper) : IQuestionAnswerChecker
    {
        public List<ICorrectAnswerDto> CheckUserAnswers(List<ICheckQuestionDto> checkQuestionDtos, List<IUserAnswerDto> userAnswers, out float userGrade)
        {
            var correctAnswers = new List<ICorrectAnswerDto>();
            userGrade = 0;

            for (int i = 0; i < userAnswers.Count; i++)
            {
                var userAnswer = userAnswers[i];
                var checkQuestionDto = checkQuestionDtos[i];

                if (userAnswer.QuestionId != checkQuestionDto.QuestionId)
                {
                    //To Do как-то по другому тут это делать
                    return new List<ICorrectAnswerDto>();
                }

                ICorrectAnswerDto correctAnswer = userAnswer switch
                {

                    TestQuestionUserAnswerDto testUserAnswer =>
                        CheckTestQuestionAnswer(testUserAnswer, (checkQuestionDto as TestQuestionCheckingDto).CorrectVariant, ref userGrade),

                    OpenQuestionUserAnswerDto openQuestionUserAnswerDto =>
                        CheckOpenQuestionAnswer(openQuestionUserAnswerDto, (checkQuestionDto as OpenQuestionCheckingDto).OpenQuestionsAnswers, ref userGrade),

                    PracticalQuestionUserAnswerDto practicalQuestionUserAnswerDto => 
                        CheckPracticalQuestionAnswer(practicalQuestionUserAnswerDto, checkQuestionDto as PracticalQuestionCheckingDto, ref userGrade),

                    _ => throw new ArgumentException("Неизвестный тип ответа")
                };

                correctAnswers.Add(correctAnswer);
            }

            return correctAnswers;
        }

        private ICorrectAnswerDto CheckTestQuestionAnswer(TestQuestionUserAnswerDto userAnswer, TestVariantDto correctTestVariant, ref float userGrade)
        {
            var correctAnswer = new TestQuestionCorrectAnswerDto
            {
                Id = userAnswer.QuestionId,
                CorrectAnswer = correctTestVariant.Content,
                AnswerCorrectness = userAnswer.UserAnswerNumberOfVariant == correctTestVariant.VariantNumber
            };

            if (correctAnswer.AnswerCorrectness)
            {
                userGrade += QuestionGrades.TestQuestionGrade;
            }

            return correctAnswer;
        }

        private ICorrectAnswerDto CheckPracticalQuestionAnswer(PracticalQuestionUserAnswerDto userAnswer, PracticalQuestionCheckingDto questionChecking, ref float userGrade)
        {
            float questionGrade = 0;

            var correctAnswer = new PracticalQuestionCorrectAnswerDto
            {
                Id = userAnswer.QuestionId,
                CorrectAnswer = questionChecking.CorrectQueryCode,
                AnswerCorrectness = false
            };

            try
            {
                var userResult = sqlHelper.ExecuteQuery(userAnswer.UserCodeAnswer);
                var rightResult = sqlHelper.ExecuteQuery(questionChecking.CorrectQueryCode);

                DataTableComparer comparer = new DataTableComparer();
                int result = comparer.Compare(userResult, rightResult);

                if (result == 0)
                {
                    correctAnswer.AnswerCorrectness = true;
                    questionGrade += QuestionGrades.PracticalQuestionGrade;
                    correctAnswer.QueryResult = userResult;

                    correctAnswer.QuestionUserGrade = questionGrade;
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
                    correctAnswer.QueryResult = userResult;
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

            userGrade += questionGrade;

            return correctAnswer;
        }
        
        // To Do обработать здесь пробелы
        private ICorrectAnswerDto CheckOpenQuestionAnswer(OpenQuestionUserAnswerDto userAnswer, List<string> openQuestionAnswerVariants, ref float userGrade)
        {
            var correctAnswer = new OpenQuestionCorrectAnswerDto
            {
                Id = userAnswer.QuestionId,
                CorrectAnswer = openQuestionAnswerVariants.FirstOrDefault(),
                AnswerCorrectness = openQuestionAnswerVariants.Any(v => v == userAnswer.UserAnswer.ToLower().Trim())
            };

            if (correctAnswer.AnswerCorrectness)
            {
                userGrade += QuestionGrades.OpenQuestionGrade;
            }

            return correctAnswer;
        }
    }
}
