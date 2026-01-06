using CourseTech.Domain.Dto.Question.CheckQuestions;
using CourseTech.Domain.Dto.Question.CorrectAnswer;
using CourseTech.Domain.Dto.Question.QuestionUserAnswer;
using CourseTech.Domain.Interfaces.Dtos.Question;
using CourseTech.Domain.Interfaces.Services.Question;
using CourseTech.Domain.Interfaces.UserQueryAnalyzers;

namespace CourseTech.Application.Services.Question.Strategies;

/// <summary>
/// Стратегия для проверки вопроса практического типа
/// Тут просто отправляем все данные в ChatGptAPI
/// </summary>
public class PracticalAnswerCheckingStrategy(IChatGptQueryAnalyzer chatGptAnalyzer) : IAnswerCheckingStrategy
{
    /// <inheritdoc cref="IAnswerCheckingStrategy.UserAnswerType"/>
    public Type UserAnswerType { get; } = typeof(PracticalQuestionUserAnswerDto);

    /// <inheritdoc cref="IAnswerCheckingStrategy.CheckAnswerAsync"/>
    public async Task<CorrectAnswerDtoBase> CheckAnswerAsync(UserAnswerDtoBase userAnswer, CheckQuestionDtoBase checkQuestion, float questionGrade)
    {
        var practicalUserAnswer = (PracticalQuestionUserAnswerDto)userAnswer;
        var questionChecking = (PracticalQuestionCheckingDto)checkQuestion;

        var userCode = practicalUserAnswer.UserCodeAnswer?.Trim();
        var correctCode = questionChecking.CorrectQueryCode?.Trim();

        var correctAnswer = new PracticalQuestionCorrectAnswerDto
        {
            Id = practicalUserAnswer.QuestionId,
            CorrectAnswer = questionChecking.CorrectQueryCode
        };

        if (string.Equals(userCode, correctCode, StringComparison.OrdinalIgnoreCase))
        {
            return new PracticalQuestionCorrectAnswerDto
            {
                Id = practicalUserAnswer.QuestionId,
                CorrectAnswer = questionChecking.CorrectQueryCode,
                AnswerCorrectness = true,
                UserGrade = questionGrade
            };
        }

        var chatGptAnalysis = await chatGptAnalyzer.AnalyzeUserQuery(
                questionGrade,
                userCode,
                correctCode);

        return new PracticalQuestionCorrectAnswerDto
        {
            Id = practicalUserAnswer.QuestionId,
            CorrectAnswer = questionChecking.CorrectQueryCode,
            ChatGptAnalysis = chatGptAnalysis,
            UserGrade = chatGptAnalysis.UserGrade,
            AnswerCorrectness = chatGptAnalysis.AnswerCorrectness
        };
    }
}
