using AutoMapper;
using CourseTech.Application.CQRS.Queries.Dtos.LessonDtoQueries;
using CourseTech.Domain.Dto.Lesson.LessonInfo;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Extensions;
using CourseTech.Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CourseTech.Application.CQRS.Handlers.QueryHandlers.DtoQueryHandlers.LessonDtoQueryHandlers;

public class GetLessonDtosHandler(IBaseRepository<Lesson> lessonRepository, IMapper mapper) : IRequestHandler<GetLessonDtosQuery, List<LessonDto>>
{
    public async Task<List<LessonDto>> Handle(GetLessonDtosQuery request, CancellationToken cancellationToken)
    {
        return await lessonRepository.GetAll()
            .AsProjected<Lesson, LessonDto>(mapper)
            .ToListAsync(cancellationToken);
    }
}
