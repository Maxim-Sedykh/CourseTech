using AutoMapper;
using CourseTech.Application.CQRS.Queries.Dtos.QuestionDtoQueries;
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
        return await questionRepository.GetAll()
            .Where(q => q.LessonId == request.LessonId)
            .Include(q => (q as TestQuestion).TestVariants)
            .Include(q => (q as OpenQuestion).AnswerVariants)
            .OrderBy(x => x.Id) //  TO DO костыль, завести поле number в questions сущности
            .Select(x => mapper.MapQuestionCheckings(x))
            .ToListAsync(cancellationToken);
    }
}
