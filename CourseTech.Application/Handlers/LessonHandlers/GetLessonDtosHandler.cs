using AutoMapper;
using CourseTech.Application.Queries.LessonQueries;
using CourseTech.Domain.Dto.Lesson.LessonInfo;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Application.Handlers.LessonHandlers
{
    public class GetLessonDtosHandler(IBaseRepository<Lesson> lessonRepository, IMapper mapper) : IRequestHandler<GetLessonDtosQuery, List<LessonDto>>
    {
        public async Task<List<LessonDto>> Handle(GetLessonDtosQuery request, CancellationToken cancellationToken)
        {
            return await lessonRepository.GetAll()
                .Select(x => mapper.Map<LessonDto>(x))
                .ToListAsync(cancellationToken);
        }
    }
}
