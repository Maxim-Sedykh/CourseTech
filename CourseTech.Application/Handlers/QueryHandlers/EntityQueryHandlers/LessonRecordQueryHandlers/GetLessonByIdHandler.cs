using AutoMapper;
using CourseTech.Application.Queries.Entities.LessonQueries;
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

namespace CourseTech.Application.Handlers.QueryHandlers.EntityQueryHandlers.LessonRecordQueryHandlers
{
    public class GetLessonByIdHandler(IBaseRepository<Lesson> lessonRepository) : IRequestHandler<GetLessonByIdQuery, Lesson>
    {
        public async Task<Lesson> Handle(GetLessonByIdQuery request, CancellationToken cancellationToken)
        {
            return await lessonRepository.GetAll().FirstOrDefaultAsync(x => x.Id == request.LessonId, cancellationToken);
        }
    }
}
