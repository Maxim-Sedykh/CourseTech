using AutoMapper;
using CourseTech.Application.CQRS.Commands.LessonCommands;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Interfaces.Databases.Repositories;
using MediatR;

namespace CourseTech.Application.CQRS.Handlers.CommandHandlers.LessonCommandHandlers;

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
