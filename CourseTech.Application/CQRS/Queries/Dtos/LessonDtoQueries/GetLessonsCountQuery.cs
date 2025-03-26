using MediatR;

namespace CourseTech.Application.CQRS.Queries.Dtos.LessonDtoQueries;

/// <summary>
/// Получение количества уроков.
/// </summary>
public class GetLessonsCountQuery : IRequest<int>;
