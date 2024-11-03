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
    /// Получение количества уроков.
    /// </summary>
    public class GetLessonsCountQuery : IRequest<int>;
}
