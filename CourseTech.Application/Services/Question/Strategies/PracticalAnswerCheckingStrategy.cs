using CourseTech.Domain.Dto.Question;
using CourseTech.Domain.Dto.Question.CheckQuestions;
using CourseTech.Domain.Dto.Question.CorrectAnswer;
using CourseTech.Domain.Dto.Question.QuestionUserAnswer;
using CourseTech.Domain.Interfaces.Dtos.Question;
using CourseTech.Domain.Interfaces.Services.Question;
using CourseTech.Domain.Interfaces.UserQueryAnalyzers;
using Microsoft.Extensions.Logging;

namespace CourseTech.Application.Services.Question.Strategies
{
    public class PracticalAnswerCheckingStrategy(IChatGptQueryAnalyzer chatGptAnalyzer, ILogger logger) : IAnswerCheckingStrategy
    {
        public Type UserAnswerType { get; } = typeof(PracticalQuestionUserAnswerDto);

        public async Task<CorrectAnswerDtoBase> CheckAnswerAsync(UserAnswerDtoBase userAnswer, CheckQuestionDtoBase checkQuestion, UserGradeDto userGrade, float questionGrade)
        {
            var practicalUserAnswer = (PracticalQuestionUserAnswerDto)userAnswer;
            var questionChecking = (PracticalQuestionCheckingDto)checkQuestion;

            var correctAnswer = new PracticalQuestionCorrectAnswerDto
            {
                Id = practicalUserAnswer.QuestionId,
                CorrectAnswer = questionChecking.CorrectQueryCode
            };

            var userQueryChatGptAnalysDto = await chatGptAnalyzer.AnalyzeUserQuery(
                    questionGrade,
                    practicalUserAnswer.UserCodeAnswer,
                    questionChecking.CorrectQueryCode);

            correctAnswer.ChatGptAnalysis = userQueryChatGptAnalysDto;

            userGrade.Grade = userQueryChatGptAnalysDto.UserGrade;

            return correctAnswer;
        }
    }
}
