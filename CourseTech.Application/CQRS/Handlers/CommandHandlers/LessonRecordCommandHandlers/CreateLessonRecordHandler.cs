using CourseTech.Application.CQRS.Commands.LessonRecordCommands;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Interfaces.Databases.Repositories;
using MediatR;

namespace CourseTech.Application.CQRS.Handlers.CommandHandlers.LessonRecordCommandHandlers;

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
