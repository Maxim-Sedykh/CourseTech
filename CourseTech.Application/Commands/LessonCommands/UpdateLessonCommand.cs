using CourseTech.Domain.Dto.Lesson.LessonInfo;
using CourseTech.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Application.Commands.LessonCommands
{
    public record UpdateLessonCommand(LessonLectureDto LessonLectureDto, Lesson Lesson) : IRequest;
}
