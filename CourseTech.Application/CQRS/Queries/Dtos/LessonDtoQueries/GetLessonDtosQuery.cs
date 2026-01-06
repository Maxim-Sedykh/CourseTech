using CourseTech.Domain.Dto.Lesson.Info;
using MediatR;

namespace CourseTech.Application.CQRS.Queries.Dtos.LessonDtoQueries;

/// <summary>
/// Получение списка всех уроков в виде LessonDto.
/// </summary>
public record GetLessonDtosQuery() : IRequest<List<LessonDto>>;
