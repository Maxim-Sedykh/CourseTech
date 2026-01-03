using CourseTech.Application.CQRS.Queries.Entities.LessonQueries;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Interfaces.Databases.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CourseTech.Application.CQRS.Handlers.QueryHandlers.EntityQueryHandlers.LessonRecordQueryHandlers;

public class GetLessonByIdHandler(IBaseRepository<Lesson> lessonRepository) : IRequestHandler<GetLessonByIdQuery, Lesson>
{
    public async Task<Lesson> Handle(GetLessonByIdQuery request, CancellationToken cancellationToken)
    {
        return await lessonRepository.GetAll().FirstOrDefaultAsync(x => x.Id == request.LessonId, cancellationToken);
    }
}
