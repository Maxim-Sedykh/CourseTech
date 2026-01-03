using AutoMapper;
using CourseTech.Application.CQRS.Queries.Dtos.QuestionDtoQueries;
using CourseTech.Domain.Entities.QuestionEntities;
using CourseTech.Domain.Entities.QuestionEntities.QuestionTypesEntities;
using CourseTech.Domain.Extensions;
using CourseTech.Domain.Interfaces.Databases.Repositories;
using CourseTech.Domain.Interfaces.Dtos.Question;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CourseTech.Application.CQRS.Handlers.QueryHandlers.DtoQueryHandlers.QuestionDtoQueryHandlers;

public class GetLessonCheckQuestionDtosHandler(IBaseRepository<BaseQuestion> questionRepository, IMapper mapper) : IRequestHandler<GetLessonCheckQuestionDtosQuery, List<CheckQuestionDtoBase>>
{
    public async Task<List<CheckQuestionDtoBase>> Handle(GetLessonCheckQuestionDtosQuery request, CancellationToken cancellationToken)
    {
        var res = await questionRepository.GetAll()
            .Where(q => q.LessonId == request.LessonId)
            .Include(q => (q as TestQuestion).TestVariants)
            .Include(q => (q as OpenQuestion).AnswerVariants)
            .OrderBy(x => x.Number)
            .Select(x => mapper.MapQuestionCheckings(x))
            .ToListAsync(cancellationToken);

        return res;
    }
}
