using AutoMapper;
using CourseTech.Domain.Dto.Question.CheckQuestions;
using CourseTech.Domain.Dto.Question.Get;
using CourseTech.Domain.Entities.QuestionEntities;
using CourseTech.Domain.Entities.QuestionEntities.QuestionTypesEntities;
using CourseTech.Domain.Interfaces.Dtos.Question;

namespace CourseTech.Domain
{
    public static class MapperExtensions
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
                case PracticalQuestion practicalQuestion:
                    return mapper.Map<OpenQuestionCheckingDto>(practicalQuestion);
                case OpenQuestion openQuestion:
                    return mapper.Map<PracticalQuestionCheckingDto>(openQuestion);
                default:
                    throw new ArgumentException("Invalid question type");
            }
        }
    }
}
