using AutoMapper;
using CourseTech.Application.CQRS.Queries.Dtos.LessonDtoQueries;
using CourseTech.Domain.Dto.Lesson.LessonInfo;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Extensions;
using CourseTech.Domain.Interfaces.Databases.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CourseTech.Application.CQRS.Handlers.QueryHandlers.DtoQueryHandlers.LessonDtoQueryHandlers;

public class GetLessonNamesHandler(IBaseRepository<Lesson> lessonRepository, IMapper mapper) : IRequestHandler<GetLessonNamesQuery, LessonNameDto[]>
{
    public async Task<LessonNameDto[]> Handle(GetLessonNamesQuery request, CancellationToken cancellationToken)
    {
        return await lessonRepository.GetAll()
                    .AsProjected<Lesson, LessonNameDto>(mapper)
                    .ToArrayAsync(cancellationToken);
    }
}
