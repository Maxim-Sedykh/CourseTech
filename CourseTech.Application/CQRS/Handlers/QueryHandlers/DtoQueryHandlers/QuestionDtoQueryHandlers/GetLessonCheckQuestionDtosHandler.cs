using AutoMapper;
using CourseTech.Application.CQRS.Queries.Dtos.QuestionDtoQueries;
using CourseTech.Domain.Dto.Question.CheckQuestions;
using CourseTech.Domain.Dto.Question.Get;
using CourseTech.Domain.Entities.QuestionEntities;
using CourseTech.Domain.Entities.QuestionEntities.QuestionTypesEntities;
using CourseTech.Domain.Extensions;
using CourseTech.Domain.Interfaces.Dtos.Question;
using CourseTech.Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CourseTech.Application.CQRS.Handlers.QueryHandlers.DtoQueryHandlers.QuestionDtoQueryHandlers;

public class GetLessonCheckQuestionDtosHandler(IBaseRepository<BaseQuestion> questionRepository, IMapper mapper) : IRequestHandler<GetLessonCheckQuestionDtosQuery, List<ICheckQuestionDto>>
{
    public async Task<List<ICheckQuestionDto>> Handle(GetLessonCheckQuestionDtosQuery request, CancellationToken cancellationToken)
    {
        var res = await questionRepository.GetAll()
            .Where(q => q.LessonId == request.LessonId)
            .Include(q => (q as TestQuestion).TestVariants)
            .Include(q => (q as OpenQuestion).AnswerVariants)
            .OrderBy(x => x.Number)
            .Select(x => mapper.MapQuestionCheckings(x))
            .ToListAsync(cancellationToken);

        if (request.IsDemoMode)
        {
            res = [.. res.Except(res.OfType<PracticalQuestionCheckingDto>())];
        }

        return res;
    }
}
