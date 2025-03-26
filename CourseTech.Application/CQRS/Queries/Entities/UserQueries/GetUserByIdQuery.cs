using CourseTech.Domain.Entities;
using MediatR;

namespace CourseTech.Application.CQRS.Queries.Entities.UserQueries;

/// <summary>
/// Получить пользователя по идентификатору.
/// </summary>
/// <param name="UserId"></param>
public record GetUserByIdQuery(Guid UserId) : IRequest<User>;
