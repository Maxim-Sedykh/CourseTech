using CourseTech.Domain.Dto.Lesson.Info;
using MediatR;

namespace CourseTech.Application.CQRS.Queries.Dtos.LessonDtoQueries;

/// <summary>
/// Получение лекционной части урока по идентификатору урока.
/// </summary>
/// <param name="LessonId"></param>
public record GetLessonLectureDtoByIdQuery(int LessonId) : IRequest<LessonLectureDto>;
