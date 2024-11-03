using CourseTech.Domain.Entities;
using MediatR;

namespace CourseTech.Application.Commands.UserTokenCommands
{
    /// <summary>
    /// Обновление Refresh-токена пользователя.
    /// </summary>
    /// <param name="UserToken"></param>
    /// <param name="RefreshToken"></param>
    public record UpdateUserTokenCommand(UserToken UserToken, string RefreshToken) : IRequest;
}
