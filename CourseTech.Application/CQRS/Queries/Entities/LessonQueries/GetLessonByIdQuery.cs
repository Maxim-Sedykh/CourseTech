using CourseTech.Domain.Entities;
using MediatR;

namespace CourseTech.Application.CQRS.Queries.Entities.LessonQueries;

/// <summary>
/// Получение урока по идентификатору.
/// </summary>
/// <param name="LessonId"></param>
public record GetLessonByIdQuery(int LessonId) : IRequest<Lesson>;
