using CourseTech.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Application.Commands.LessonRecordCommands
{
    public record CreateLessonRecordCommand(Guid UserId, int LessonId, float UserGrade) : IRequest;
}
