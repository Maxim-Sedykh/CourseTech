using AutoMapper;
using CourseTech.Application.CQRS.Queries.Dtos.LessonRecordDtoQueries;
using CourseTech.Domain.Dto.LessonRecord;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Extensions;
using CourseTech.Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CourseTech.Application.CQRS.Handlers.QueryHandlers.DtoQueryHandlers.LessonRecordDtoQueryHandlers;

public class GetLessonRecordDtosByUserIdHandler(IBaseRepository<LessonRecord> lessonRecordRepository, IMapper mapper) : IRequestHandler<GetLessonRecordDtosByUserIdQuery, LessonRecordDto[]>
{
    public async Task<LessonRecordDto[]> Handle(GetLessonRecordDtosByUserIdQuery request, CancellationToken cancellationToken)
    {
        return await lessonRecordRepository.GetAll()
                    .Where(x => x.UserId == request.UserId && !x.IsDemo)
                    .Include(x => x.Lesson)
                    .AsProjected<LessonRecord, LessonRecordDto>(mapper)
                    .ToArrayAsync(cancellationToken);
    }
}
