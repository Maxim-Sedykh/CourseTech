using MediatR;

namespace CourseTech.Application.CQRS.Commands.UserTokenCommands;

/// <summary>
/// Создание токена для пользователя.
/// </summary>
/// <param name="UserId"></param>
/// <param name="RefreshToken"></param>
/// <param name="RefreshTokenValidityInDays"></param>
public record CreateUserTokenCommand(Guid UserId, string RefreshToken, int RefreshTokenValidityInDays) : IRequest;
