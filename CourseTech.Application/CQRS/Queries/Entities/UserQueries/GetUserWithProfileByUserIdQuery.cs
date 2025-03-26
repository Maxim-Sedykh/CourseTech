using CourseTech.Domain.Entities;
using MediatR;

namespace CourseTech.Application.CQRS.Queries.Entities.UserQueries;

/// <summary>
/// Получение пользователя с профилем по идентификатору пользователя.
/// </summary>
/// <param name="UserId"></param>
public record GetUserWithProfileByUserIdQuery(Guid UserId) : IRequest<User>;
