using CourseTech.DAL.Views;
using MediatR;

namespace CourseTech.Application.CQRS.Queries.Views;

/// <summary>
/// Получение представления <see cref="QuestionTypeGrade"/>
/// </summary>
/// <param name="UserId"></param>
public record GetQuestionTypeGradeQuery() : IRequest<List<QuestionTypeGrade>>;
