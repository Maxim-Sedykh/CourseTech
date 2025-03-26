using CourseTech.Domain.Entities;
using MediatR;

namespace CourseTech.Application.CQRS.Queries.Entities.UserProfileQueries;

/// <summary>
/// Получение профиля пользователя по идентификатору пользователя.
/// </summary>
/// <param name="UserId"></param>
public record GetProfileByUserIdQuery(Guid UserId) : IRequest<UserProfile>;
