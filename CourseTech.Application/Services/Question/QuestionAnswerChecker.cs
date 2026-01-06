using CourseTech.Domain.Dto.Question;
using CourseTech.Domain.Interfaces.Dtos.Question;
using CourseTech.Domain.Interfaces.Helpers;
using CourseTech.Domain.Interfaces.Services.Question;
using CourseTech.Domain.Views;
using Serilog;

namespace CourseTech.Application.Services.Question;

/// <summary>
/// Cервис для проверки пользовательский ответов на вопросы в практической части урока.
/// </summary>
public class QuestionAnswerChecker : IQuestionAnswerChecker
{
    private readonly IAnswerCheckingStrategyFactory _answerCheckingStrategyFactory;
    private readonly ILogger _logger;

    public QuestionAnswerChecker(IAnswerCheckingStrategyFactory answerCheckingStrategyFactory, ILogger logger)
    {
        _answerCheckingStrategyFactory = answerCheckingStrategyFactory;
        _logger = logger;
    }

    /// <inheritdoc cref="IQuestionAnswerChecker.CheckUserAnswers"/>
    public async Task<List<CorrectAnswerDtoBase>> CheckUserAnswers(
        List<CheckQuestionDtoBase> checkQuestionDtos,
        List<UserAnswerDtoBase> userAnswers,
        UserGradeDto userGrade,
        List<QuestionTypeGrade> questionTypeGrade)
    {
        var currentGradesMapping = questionTypeGrade
            .ToDictionary(x => x.QuestionTypeName, x => x.Grade);

        var correctAnswers = new List<CorrectAnswerDtoBase>();
        userGrade.Grade = 0;

        var tasks = new List<Task<CorrectAnswerDtoBase>>();

        for (int i = 0; i < userAnswers.Count; i++)
        {
            var userAnswer = userAnswers[i];
            var checkQuestionDto = checkQuestionDtos[i];

            if (userAnswer.QuestionId != checkQuestionDto.QuestionId)
            {
                return [];
            }

            tasks.Add(CheckAnswer(userAnswer, checkQuestionDto, currentGradesMapping));
        }

        CorrectAnswerDtoBase[] results = [];
        var allTasks = Task.WhenAll(tasks);

        try
        {
            results = await allTasks;
        }
        catch (Exception ex)
        {
            if (allTasks.Exception != null)
            {
                foreach (var innerEx in allTasks.Exception.Flatten().InnerExceptions)
                {
                    _logger.Error(innerEx, "Error occurred while checking a question");
                }
            }
            else
            {
                _logger.Error(ex, "An unexpected error occurred");
            }

            throw;
        }

        correctAnswers.AddRange(results);

        userGrade.Grade = results.Sum(r => r.UserGrade);

        return correctAnswers;
    }

    /// <summary>
    /// Проверить вопрос
    /// </summary>
    /// <param name="userAnswer">Ответ пользователя</param>
    /// <param name="checkQuestionDto">Данные для проверки вопроса</param>
    /// <param name="gradesMapping">Маппинг типов вопросов и оценки за них</param>
    /// <returns>Данные о проверке вопроса</returns>
    private Task<CorrectAnswerDtoBase> CheckAnswer(UserAnswerDtoBase userAnswer,
        CheckQuestionDtoBase checkQuestionDto,
        Dictionary<string, float> gradesMapping)
    {
        var answerType = userAnswer.GetType();

        var strategy = _answerCheckingStrategyFactory.CreateAnswerCheckingStrategy(answerType);

        var questionTypeName = answerType.Name.Replace("UserAnswerDto", "");
        var questionGrade = gradesMapping.GetValueOrDefault(questionTypeName, 0f);

        return strategy.CheckAnswerAsync(userAnswer, checkQuestionDto, questionGrade);
    }
}
