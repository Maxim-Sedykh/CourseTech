using CourseTech.Domain.Dto.Lesson.LessonInfo;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Application.CQRS.Queries.Dtos.LessonDtoQueries
{
    /// <summary>
    /// Получение названий всех уроков.
    /// </summary>
    public record GetLessonNamesQuery() : IRequest<LessonNameDto[]>;
}
