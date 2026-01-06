using CourseTech.Domain.Dto.Question;
using CourseTech.Domain.Dto.Question.CheckQuestions;
using CourseTech.Domain.Dto.Question.CorrectAnswer;
using CourseTech.Domain.Dto.Question.QuestionUserAnswer;
using CourseTech.Domain.Interfaces.Dtos.Question;
using CourseTech.Domain.Interfaces.Services.Question;

namespace CourseTech.Application.Services.Question.Strategies;

/// <summary>
/// Стратегия для проверки вопроса открытого типа
/// </summary>
public class OpenAnswerCheckingStrategy : IAnswerCheckingStrategy
{
    /// <inheritdoc cref="IAnswerCheckingStrategy.UserAnswerType"/>
    public Type UserAnswerType { get; } = typeof(OpenQuestionUserAnswerDto);

    /// <inheritdoc cref="IAnswerCheckingStrategy.CheckAnswerAsync"/>
    public Task<CorrectAnswerDtoBase> CheckAnswerAsync(UserAnswerDtoBase userAnswer, CheckQuestionDtoBase checkQuestion, float questionGrade)
    {
        var openUserAnswer = (OpenQuestionUserAnswerDto)userAnswer;
        var openQuestionAnswerVariants = ((OpenQuestionCheckingDto)checkQuestion).OpenQuestionsAnswers;

        string normalizedUserAnswer = openUserAnswer.UserAnswer.ToLower().Trim();

        var result = new OpenQuestionCorrectAnswerDto
        {
            Id = openUserAnswer.QuestionId,
            CorrectAnswer = openQuestionAnswerVariants.FirstOrDefault(),
            AnswerCorrectness = openQuestionAnswerVariants.Any(v => v.Equals(normalizedUserAnswer, StringComparison.OrdinalIgnoreCase)),
        };

        if (result.AnswerCorrectness)
        {
            result.UserGrade = questionGrade;
        }

        return Task.FromResult((CorrectAnswerDtoBase)result);
    }
}
