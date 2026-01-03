using CourseTech.DAL.Views;
using CourseTech.Domain.Dto.Question;
using CourseTech.Domain.Interfaces.Dtos.Question;
using CourseTech.Domain.Interfaces.Helpers;
using CourseTech.Domain.Interfaces.Services.Question;

namespace CourseTech.Application.Services.Question;

public class QuestionAnswerChecker : IQuestionAnswerChecker
{
    private readonly IAnswerCheckingStrategyFactory _answerCheckingStrategyFactory;
    private readonly Dictionary<string, float> _questionGrades;

    public QuestionAnswerChecker(IAnswerCheckingStrategyFactory answerCheckingStrategyFactory)
    {
        _answerCheckingStrategyFactory = answerCheckingStrategyFactory;
    }


    public async Task<List<CorrectAnswerDtoBase>> CheckUserAnswers(
        List<CheckQuestionDtoBase> checkQuestionDtos,
        List<UserAnswerDtoBase> userAnswers,
        UserGradeDto userGrade,
        List<QuestionTypeGrade> questionTypeGrade)
    {
        _questionGrades.Clear();
        foreach (var grade in questionTypeGrade)
        {
            _questionGrades[grade.QuestionTypeName] = grade.Grade;
        }

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

            tasks.Add(CheckAnswer(userAnswer, checkQuestionDto, userGrade));
        }

        var results = await Task.WhenAll(tasks);
        correctAnswers.AddRange(results);

        return correctAnswers;
    }

    private Task<CorrectAnswerDtoBase> CheckAnswer(UserAnswerDtoBase userAnswer, CheckQuestionDtoBase checkQuestionDto, UserGradeDto userGrade)
    {
        var answerType = userAnswer.GetType();

        var strategy = _answerCheckingStrategyFactory.CreateAnswerCheckingStrategy(answerType);

        var questionTypeName = answerType.Name.Replace("UserAnswerDto", "");
        var questionGrade = _questionGrades.GetValueOrDefault(questionTypeName, 0f);

        return strategy.CheckAnswerAsync(userAnswer, checkQuestionDto, userGrade, questionGrade);
    }
}
