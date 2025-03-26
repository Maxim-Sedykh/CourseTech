using CourseTech.Domain.Interfaces.Dtos.Question;
using MediatR;

namespace CourseTech.Application.CQRS.Queries.Dtos.QuestionDtoQueries;

/// <summary>
/// Получение списка CheckQuestionDto для вопросов по определённому уроку.
/// </summary>
/// <param name="LessonId"></param>
public record GetLessonCheckQuestionDtosQuery(int LessonId) : IRequest<List<ICheckQuestionDto>>;
