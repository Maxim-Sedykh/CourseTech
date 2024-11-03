using CourseTech.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Application.Queries.LessonQueries
{
    /// <summary>
    /// Получение урока по идентификатору.
    /// </summary>
    /// <param name="LessonId"></param>
    public record GetLessonByIdQuery(int LessonId) : IRequest<Lesson>;
}
