using AutoMapper;
using CourseTech.Application.Commands.Reviews;
using CourseTech.Application.CQRS.Commands.LessonCommands;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Interfaces.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Application.CQRS.Handlers.CommandHandlers.LessonCommandHandlers
{
    public class UpdateLessonHandler(IBaseRepository<Lesson> lessonRepository, IMapper mapper) : IRequestHandler<UpdateLessonCommand>
    {
        public async Task Handle(UpdateLessonCommand request, CancellationToken cancellationToken)
        {
            var lesson = request.Lesson;

            mapper.Map(request.LessonLectureDto, lesson);

            lessonRepository.Update(lesson);
            await lessonRepository.SaveChangesAsync(cancellationToken);
        }
    }
}
