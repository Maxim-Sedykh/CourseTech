using CourseTech.Domain.Dto.Lesson.LessonInfo;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Application.Queries.LessonQueries
{
    public record GetLessonNamesQuery() : IRequest<LessonNameDto[]>;
}
