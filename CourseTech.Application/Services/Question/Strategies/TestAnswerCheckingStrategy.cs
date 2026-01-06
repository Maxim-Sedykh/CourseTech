using CourseTech.Domain.Dto.Question.CheckQuestions;
using CourseTech.Domain.Dto.Question.CorrectAnswer;
using CourseTech.Domain.Dto.Question.QuestionUserAnswer;
using CourseTech.Domain.Interfaces.Dtos.Question;
using CourseTech.Domain.Interfaces.Services.Question;

namespace CourseTech.Application.Services.Question.Strategies;

/// <summary>
/// Стратегия для проверки вопросов тестового типа
/// </summary>
public class TestAnswerCheckingStrategy : IAnswerCheckingStrategy
{
    /// <inheritdoc cref="IAnswerCheckingStrategy.UserAnswerType"/>
    public Type UserAnswerType { get; } = typeof(TestQuestionUserAnswerDto);

    /// <inheritdoc cref="IAnswerCheckingStrategy.CheckAnswerAsync"/>
    public Task<CorrectAnswerDtoBase> CheckAnswerAsync(UserAnswerDtoBase userAnswer, CheckQuestionDtoBase checkQuestion, float questionGrade)
    {
        var testUserAnswer = (TestQuestionUserAnswerDto)userAnswer;
        var correctTestVariant = ((TestQuestionCheckingDto)checkQuestion).CorrectVariant;

        bool isCorrect = testUserAnswer.UserAnswerNumberOfVariant == correctTestVariant.VariantNumber;

        var result = new TestQuestionCorrectAnswerDto
        {
            Id = testUserAnswer.QuestionId,
            CorrectAnswer = correctTestVariant.Content,
            AnswerCorrectness = isCorrect
        };

        if (isCorrect)
        {
            result.UserGrade += questionGrade;
        }

        return Task.FromResult((CorrectAnswerDtoBase)result);
    }
}
