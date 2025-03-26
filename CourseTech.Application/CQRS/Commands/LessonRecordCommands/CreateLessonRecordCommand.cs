using MediatR;

namespace CourseTech.Application.CQRS.Commands.LessonRecordCommands;

/// <summary>
/// Создание записи о прохождении урока пользователем.
/// </summary>
/// <param name="UserId"></param>
/// <param name="LessonId"></param>
/// <param name="UserGrade"></param>
public record CreateLessonRecordCommand(Guid UserId, int LessonId, float UserGrade) : IRequest;
