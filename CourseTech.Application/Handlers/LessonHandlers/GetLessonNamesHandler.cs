using AutoMapper;
using CourseTech.Application.Queries.LessonQueries;
using CourseTech.Application.Queries.LessonRecordQueries;
using CourseTech.Domain.Dto.Lesson.LessonInfo;
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
using static StackExchange.Redis.Role;

namespace CourseTech.Application.Handlers.LessonHandlers
{
    public class GetLessonNamesHandler(IBaseRepository<Lesson> lessonRepository, IMapper mapper) : IRequestHandler<GetLessonNamesQuery, LessonNameDto[]>
    {
        public async Task<LessonNameDto[]> Handle(GetLessonNamesQuery request, CancellationToken cancellationToken)
        {
            return await lessonRepository.GetAll()
                        .Select(x => mapper.Map<LessonNameDto>(x))
                        .ToArrayAsync(cancellationToken);
        }
    }
}
