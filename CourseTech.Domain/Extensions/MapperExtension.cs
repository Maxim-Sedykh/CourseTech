using AutoMapper;
using CourseTech.Domain.Dto.Question.CheckQuestions;
using CourseTech.Domain.Dto.Question.Get;
using CourseTech.Domain.Entities.QuestionEntities;
using CourseTech.Domain.Entities.QuestionEntities.QuestionTypesEntities;
using CourseTech.Domain.Interfaces.Dtos.Question;

namespace CourseTech.Domain.Extensions;

/// <summary>
/// Расширение для Automapper'а.
/// </summary>
public static class MapperExtension
{
    /// <summary>
    /// Метод для маппинга сущности Question в разные типы QuestionDto.
    /// </summary>
    /// <param name="mapper"></param>
    /// <param name="question"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static IQuestionDto MapQuestion(this IMapper mapper, BaseQuestion question)
    {
        switch (question)
        {
            case TestQuestion testQuestion:
                return mapper.Map<TestQuestionDto>(testQuestion);
            case PracticalQuestion practicalQuestion:
                return mapper.Map<PracticalQuestionDto>(practicalQuestion);
            case OpenQuestion openQuestion:
                return mapper.Map<OpenQuestionDto>(openQuestion);
            default:
                throw new ArgumentException("Invalid question type");
        }
    }

    /// <summary>
    /// Метод для маппинга сущности Question в разные типы QuestionChekingDto.
    /// </summary>
    /// <param name="mapper"></param>
    /// <param name="question"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static ICheckQuestionDto MapQuestionCheckings(this IMapper mapper, BaseQuestion question)
    {
        switch (question)
        {
            case TestQuestion testQuestion:
                return mapper.Map<TestQuestionCheckingDto>(testQuestion);
            case OpenQuestion openQuestion:
                return mapper.Map<OpenQuestionCheckingDto>(openQuestion);
            case PracticalQuestion practicalQuestion:
                return mapper.Map<PracticalQuestionCheckingDto>(practicalQuestion);
            default:
                throw new ArgumentException("Invalid question type");
        }
    }
}
