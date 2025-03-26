using CourseTech.Domain.Dto.LessonRecord;
using MediatR;

namespace CourseTech.Application.CQRS.Queries.Dtos.LessonRecordDtoQueries;

/// <summary>
/// Получение записей о прохождении уроков в виде LessonRecordDto, для определённого пользователя по его идентификатору.
/// </summary>
/// <param name="UserId"></param>
public record GetLessonRecordDtosByUserIdQuery(Guid UserId) : IRequest<LessonRecordDto[]>;
