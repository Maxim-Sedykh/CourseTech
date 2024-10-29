using CourseTech.Domain.Entities;
using MediatR;

namespace CourseTech.Application.Commands.UserTokenCommands
{
    public record UpdateUserTokenCommand(UserToken UserToken, string RefreshToken) : IRequest;
}
