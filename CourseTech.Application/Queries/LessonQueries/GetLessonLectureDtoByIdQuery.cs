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
    /// Получение лекционной части урока по идентификатору урока.
    /// </summary>
    /// <param name="LessonId"></param>
    public record GetLessonLectureDtoByIdQuery(int LessonId) : IRequest<LessonLectureDto>;
}
