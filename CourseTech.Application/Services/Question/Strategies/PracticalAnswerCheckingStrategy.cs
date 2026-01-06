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
        correctAnswer.UserGrade = userQueryChatGptAnalysDto.UserGrade;

        return correctAnswer;
    }
}
