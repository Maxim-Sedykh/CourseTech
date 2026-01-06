using CourseTech.Domain.Dto.Lesson.Info;
using MediatR;

namespace CourseTech.Application.CQRS.Queries.Dtos.LessonDtoQueries;

/// <summary>
/// Получение названий всех уроков.
/// </summary>
public record GetLessonNamesQuery() : IRequest<LessonNameDto[]>;
