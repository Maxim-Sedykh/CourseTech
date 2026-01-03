using AutoMapper;
using CourseTech.Application.CQRS.Queries.Dtos.QuestionDtoQueries;
using CourseTech.Domain.Dto.Question.Get;
using CourseTech.Domain.Entities.QuestionEntities;
using CourseTech.Domain.Entities.QuestionEntities.QuestionTypesEntities;
using CourseTech.Domain.Extensions;
using CourseTech.Domain.Interfaces.Databases.Repositories;
using CourseTech.Domain.Interfaces.Dtos.Question;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CourseTech.Application.CQRS.Handlers.QueryHandlers.DtoQueryHandlers.QuestionDtoQueryHandlers;

public class GetLessonQuestionDtosHandler(IBaseRepository<BaseQuestion> questionRepository, IMapper mapper) : IRequestHandler<GetLessonQuestionDtosQuery, List<QuestionDtoBase>>
{
    public async Task<List<QuestionDtoBase>> Handle(GetLessonQuestionDtosQuery request, CancellationToken cancellationToken)
    {
        var res = await questionRepository.GetAll()
            .Include(q => (q as TestQuestion).TestVariants)
            .Where(q => q.LessonId == request.LessonId)
            .Select(q => mapper.MapQuestion(q))
            .ToListAsync(cancellationToken);

        return res;
    }
}
