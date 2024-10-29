using AutoMapper;
using CourseTech.Application.Queries.LessonQueries;
using CourseTech.Application.Queries.LessonRecordQueries;
using CourseTech.Domain.Dto.LessonRecord;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Application.Handlers.LessonRecordHandlers
{
    public class GetLessonRecordDtosByUserIdHandler(IBaseRepository<LessonRecord> lessonRecordRepository, IMapper mapper) : IRequestHandler<GetLessonRecordDtosByUserIdQuery, LessonRecordDto[]>
    {
        public async Task<LessonRecordDto[]> Handle(GetLessonRecordDtosByUserIdQuery request, CancellationToken cancellationToken)
        {
            return await lessonRecordRepository.GetAll()
                        .Where(x => x.UserId == request.UserId)
                        .Include(x => x.Lesson)
                        .Select(x => mapper.Map<LessonRecordDto>(x))
                        .ToArrayAsync(cancellationToken);
        }
    }
}
