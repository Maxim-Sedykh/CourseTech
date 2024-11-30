using CourseTech.Application.Commands.LessonRecordCommands;
using CourseTech.Application.Commands.Reviews;
using CourseTech.DAL.Repositories;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Interfaces.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Application.Handlers.CommandHandlers.LessonRecordCommandHandlers
{
    public class CreateLessonRecordHandler(IBaseRepository<LessonRecord> lessonRecordRepository) : IRequestHandler<CreateLessonRecordCommand>
    {
        public async Task Handle(CreateLessonRecordCommand request, CancellationToken cancellationToken)
        {
            await lessonRecordRepository.CreateAsync(new LessonRecord()
            {
                LessonId = request.LessonId,
                UserId = request.UserId,
                Mark = request.UserGrade
            });
        }
    }
}
