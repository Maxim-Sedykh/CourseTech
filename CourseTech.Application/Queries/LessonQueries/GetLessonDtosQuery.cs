using CourseTech.Domain.Dto.Lesson.LessonInfo;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Application.Queries.LessonQueries
{
    /// <summary>
    /// Получение списка всех уроков в виде LessonDto.
    /// </summary>
    public record GetLessonDtosQuery(): IRequest<List<LessonDto>>;
}
