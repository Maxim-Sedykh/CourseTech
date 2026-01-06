using CourseTech.Domain.Dto.Lesson.Info;
using CourseTech.Domain.Entities;
using MediatR;

namespace CourseTech.Application.CQRS.Commands.LessonCommands;

/// <summary>
/// Обновление сущности "Урок".
/// </summary>
/// <param name="LessonLectureDto"></param>
/// <param name="Lesson"></param>
public record UpdateLessonCommand(LessonLectureDto LessonLectureDto, Lesson Lesson) : IRequest;
