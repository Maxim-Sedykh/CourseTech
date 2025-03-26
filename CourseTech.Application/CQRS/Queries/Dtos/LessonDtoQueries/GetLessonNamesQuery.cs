using CourseTech.Domain.Dto.Lesson.LessonInfo;
using MediatR;

namespace CourseTech.Application.CQRS.Queries.Dtos.LessonDtoQueries;

/// <summary>
/// Получение названий всех уроков.
/// </summary>
public record GetLessonNamesQuery() : IRequest<LessonNameDto[]>;
