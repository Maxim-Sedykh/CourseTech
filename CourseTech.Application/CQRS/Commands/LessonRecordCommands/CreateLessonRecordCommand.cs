using CourseTech.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Application.CQRS.Commands.LessonRecordCommands
{
    /// <summary>
    /// Создание записи о прохождении урока пользователем.
    /// </summary>
    /// <param name="UserId"></param>
    /// <param name="LessonId"></param>
    /// <param name="UserGrade"></param>
    public record CreateLessonRecordCommand(Guid UserId, int LessonId, float UserGrade) : IRequest;
}
