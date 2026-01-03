using CourseTech.Domain.Interfaces.Dtos.Question;
using MediatR;

namespace CourseTech.Application.CQRS.Queries.Dtos.QuestionDtoQueries;

/// <summary>
/// Получение вопросов урока в виде QuestionDto
/// </summary>
/// <param name="LessonId"></param>
public record GetLessonQuestionDtosQuery(int LessonId) : IRequest<List<QuestionDtoBase>>;
