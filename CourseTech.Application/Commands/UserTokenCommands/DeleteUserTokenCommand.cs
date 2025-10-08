using CourseTech.Domain.Entities.UserRelated;
using MediatR;

namespace CourseTech.Application.Commands.UserTokenCommands;

/// <summary>
/// Удаление токена пользователя.
/// </summary>
/// <param name="UserToken"></param>
public record DeleteUserTokenCommand(UserToken UserToken) : IRequest;
