using CourseTech.Domain.Entities;
using MediatR;

namespace CourseTech.Application.CQRS.Queries.Entities.UserTokenQueries;

/// <summary>
/// Получение токена пользователя по идентификатору пользователя.
/// </summary>
/// <param name="UserId"></param>
public record GetUserTokenByUserIdQuery(Guid UserId) : IRequest<UserToken>;
