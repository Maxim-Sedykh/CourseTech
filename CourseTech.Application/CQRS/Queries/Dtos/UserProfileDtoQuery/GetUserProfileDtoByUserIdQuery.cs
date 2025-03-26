using CourseTech.Domain.Dto.UserProfile;
using MediatR;

namespace CourseTech.Application.CQRS.Queries.Dtos.UserProfileDtoQuery;

/// <summary>
/// Получение UserProfileDto по идентификатору пользователя.
/// </summary>
/// <param name="UserId"></param>
public record GetUserProfileDtoByUserIdQuery(Guid UserId) : IRequest<UserProfileDto>;
