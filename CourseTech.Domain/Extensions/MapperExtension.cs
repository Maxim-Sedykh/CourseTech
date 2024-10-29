using AutoMapper;
using CourseTech.Domain.Dto.Question.CheckQuestions;
using CourseTech.Domain.Dto.Question.Get;
using CourseTech.Domain.Entities.QuestionEntities;
using CourseTech.Domain.Entities.QuestionEntities.QuestionTypesEntities;
using CourseTech.Domain.Interfaces.Dtos.Question;

namespace CourseTech.Domain.Extensions;

public static class MapperExtension
{
    public static IQuestionDto MapQuestion(this IMapper mapper, Question question)
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

    public static ICheckQuestionDto MapQuestionCheckings(this IMapper mapper, Question question)
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
