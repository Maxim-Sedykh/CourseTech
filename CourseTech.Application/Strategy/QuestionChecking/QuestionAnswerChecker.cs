using CourseTech.Application.Resources;
using CourseTech.DAL.Views;
using CourseTech.Domain.Dto.Question;
using CourseTech.Domain.Dto.Question.QuestionUserAnswer;
using CourseTech.Domain.Interfaces.Databases;
using CourseTech.Domain.Interfaces.Dtos.Question;
using CourseTech.Domain.Interfaces.Helpers;
using CourseTech.Domain.Interfaces.Strategy;
using CourseTech.Domain.Interfaces.UserQueryAnalyzers;
using Microsoft.Extensions.Logging;

namespace CourseTech.Application.Strategy.QuestionChecking;

public class QuestionAnswerChecker : IQuestionAnswerChecker
{
    private readonly Dictionary<Type, IAnswerCheckingStrategy> _strategies;
    private readonly Dictionary<string, float> _questionGrades;

    public QuestionAnswerChecker(
        IChatGptQueryAnalyzer chatGptQueryAnalyzer,
        ISqlQueryProvider sqlProvider,
        ILogger logger)
    {
        _strategies = new Dictionary<Type, IAnswerCheckingStrategy>
        {
            [typeof(TestQuestionUserAnswerDto)] = new TestAnswerCheckingStrategy(),
            [typeof(OpenQuestionUserAnswerDto)] = new OpenAnswerCheckingStrategy(),
            [typeof(PracticalQuestionUserAnswerDto)] = new PracticalAnswerCheckingStrategy(sqlProvider, chatGptQueryAnalyzer, logger)
        };

        _questionGrades = [];
    }

    public async Task<List<ICorrectAnswerDto>> CheckUserAnswers(
        List<ICheckQuestionDto> checkQuestionDtos,
        List<IUserAnswerDto> userAnswers,
        UserGradeDto userGrade,
        List<QuestionTypeGrade> questionTypeGrade)
    {
        _questionGrades.Clear();
        foreach (var grade in questionTypeGrade)
        {
            _questionGrades[grade.QuestionTypeName] = grade.Grade;
        }

        var correctAnswers = new List<ICorrectAnswerDto>();
        userGrade.Grade = 0;

        var tasks = new List<Task<ICorrectAnswerDto>>();

        for (int i = 0; i < userAnswers.Count; i++)
        {
            var userAnswer = userAnswers[i];
            var checkQuestionDto = checkQuestionDtos[i];

            if (userAnswer.QuestionId != checkQuestionDto.QuestionId)
            {
                return [];
            }

            tasks.Add(CheckAnswer(userAnswer, checkQuestionDto, userGrade));
        }

        var results = await Task.WhenAll(tasks);
        correctAnswers.AddRange(results);

        return correctAnswers;
    }

    private Task<ICorrectAnswerDto> CheckAnswer(IUserAnswerDto userAnswer, ICheckQuestionDto checkQuestionDto, UserGradeDto userGrade)
    {
        var answerType = userAnswer.GetType();

        if (!_strategies.TryGetValue(answerType, out var strategy))
        {
            throw new ArgumentException(ErrorMessage.InvalidQuestionType);
        }

        var questionTypeName = answerType.Name.Replace("UserAnswerDto", "");
        var questionGrade = _questionGrades.GetValueOrDefault(questionTypeName, 0f);

        return strategy.CheckAnswerAsync(userAnswer, checkQuestionDto, userGrade, questionGrade);
    }
}
