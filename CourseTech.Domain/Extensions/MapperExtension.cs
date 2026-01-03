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
    public static QuestionDtoBase MapQuestion(this IMapper mapper, BaseQuestion question)
    {
        return question switch
        {
            TestQuestion testQuestion => mapper.Map<TestQuestionDto>(testQuestion),
            PracticalQuestion practicalQuestion => mapper.Map<PracticalQuestionDto>(practicalQuestion),
            OpenQuestion openQuestion => mapper.Map<OpenQuestionDto>(openQuestion),
            _ => throw new ArgumentException("Invalid question type"),
        };
    }

    /// <summary>
    /// Метод для маппинга сущности Question в разные типы QuestionChekingDto.
    /// </summary>
    /// <param name="mapper"></param>
    /// <param name="question"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static CheckQuestionDtoBase MapQuestionCheckings(this IMapper mapper, BaseQuestion question)
    {
        return question switch
        {
            TestQuestion testQuestion => mapper.Map<TestQuestionCheckingDto>(testQuestion),
            OpenQuestion openQuestion => mapper.Map<OpenQuestionCheckingDto>(openQuestion),
            PracticalQuestion practicalQuestion => mapper.Map<PracticalQuestionCheckingDto>(practicalQuestion),
            _ => throw new ArgumentException("Invalid question type"),
        };
    }
}
