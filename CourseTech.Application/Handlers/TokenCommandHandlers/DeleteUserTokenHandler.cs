using CourseTech.Application.Commands.UserTokenCommands;
using CourseTech.Domain.Interfaces.Repositories;
using MediatR;

namespace CourseTech.Application.Handlers.TokenCommandHandlers;

public class DeleteUserTokenHandler(IUserTokenRepository userTokenRepository) : IRequestHandler<DeleteUserTokenCommand>
{
    public Task Handle(DeleteUserTokenCommand request, CancellationToken cancellationToken)
    {
        userTokenRepository.Remove(request.UserToken);

        return Task.CompletedTask;
    }
}
