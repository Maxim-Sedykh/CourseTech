using CourseTech.Domain.Constants;
using CourseTech.Domain.Dto.Question.Pass;
using CourseTech.Domain.Dto.Question.QuestionUserAnswer;
using CourseTech.Domain.Dto.TestVariant;
using CourseTech.Domain.Helpers;
using CourseTech.Domain.Interfaces.Dtos.Question;
using CourseTech.Domain.Interfaces.Graph;
using CourseTech.Domain.Interfaces.Helpers;

namespace CourseTech.Application.Helpers
{
    public class QuestionAnswerChecker(IQueryGraphAnalyzer queryGraphAnalyzer) : IQuestionAnswerChecker
    {
        public ICorrectAnswerDto CheckTestQuestionAnswer(TestQuestionUserAnswerDto userAnswer, TestVariantDto correctTestVariantDto, ref float userGrade)
        {
            var correctAnswer = new TestQuestionCorrectAnswerDto
            {
                Id = userAnswer.QuestionId,
                DisplayAnswer = correctTestVariantDto.DisplayAnswer
            };

            if (userAnswer.UserAnswerNumberOfVariant == correctTestVariantDto.Number)
            {
                correctAnswer.AnswerCorrectness = true;

                userGrade += QuestionGradeConstants.TestQuestionGrade;
            }
            else
            {
                correctAnswer.AnswerCorrectness = false;
            }

            return correctAnswer;
        }

        public ICorrectAnswerDto CheckPracticalQuestionAnswer(PracticalQuestionUserAnswerDto userAnswer,
                                                                string rightQueryCode,
                                                                List<string> questionKeywords,
                                                                out float questionGrade)
        {
            questionGrade = 0;

            var correctAnswer = new PracticalQuestionCorrectAnswerDto
            {
                Id = userAnswer.QuestionId,
                CorrectAnswer = rightQueryCode,
            };

            var remarks = new List<string>();
            try
            {
                var userResult = SqlHelper.ExecuteQuery(userAnswer.UserCodeAnswer);
                var rightResult = SqlHelper.ExecuteQuery(rightQueryCode);

                DataTableComparer comparer = new DataTableComparer();
                int result = comparer.Compare(userResult, rightResult);



                if (result == 0)
                {
                    correctAnswer.AnswerCorrectness = true;

                    questionGrade += QuestionGradeConstants.PracticalQuestionGrade;

                    if (userResult != null)
                    {
                        correctAnswer.QueryResult = userResult;
                    }

                }
                else
                {
                    queryGraphAnalyzer.CalculateUserQueryScore(userAnswer.UserCodeAnswer.ToLower(),
                        questionKeywords,
                        out float practicalQuestionGrade,
                        out remarks);

                    correctAnswer.AnswerCorrectness = false;
                    correctAnswer.Remarks = remarks;

                    correctAnswer.QuestionUserGrade = practicalQuestionGrade;

                    questionGrade += practicalQuestionGrade;
                }

            }
            catch (Exception)
            {
                if (userAnswer.UserCodeAnswer != null)
                {
                    queryGraphAnalyzer.CalculateUserQueryScore(userAnswer.UserCodeAnswer.ToLower(),
                        questionKeywords,
                        out float practicalQuestionGrade,
                        out remarks);

                    correctAnswer.AnswerCorrectness = false;
                    correctAnswer.Remarks = remarks;
                }
            }

            return correctAnswer;
        }

        public ICorrectAnswerDto CheckOpenQuestionAnswer(OpenQuestionUserAnswerDto userAnswer, List<string> openQuestionAnswerVariants, ref float userGrade)
        {
            var correctAnswer = new OpenQuestionCorrectAnswerDto
            {
                Id = userAnswer.QuestionId,
                CorrectAnswer = openQuestionAnswerVariants.FirstOrDefault()
            };

            if (openQuestionAnswerVariants.Any(v => v == userAnswer.UserAnswer))
            {
                correctAnswer.AnswerCorrectness = true;
                userGrade += QuestionGradeConstants.OpenQuestionGrade;
            }
            else
            {
                correctAnswer.AnswerCorrectness = false;
            }

            return correctAnswer;
        }
    }
}
