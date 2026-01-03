using CourseTech.Application.CQRS.Commands.UserTokenCommands;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Interfaces.Databases.Repositories;
using MediatR;

namespace CourseTech.Application.CQRS.Handlers.CommandHandlers.TokenCommandHandlers;

public class DeleteUserTokenHandler(IBaseRepository<UserToken> userTokenRepository) : IRequestHandler<DeleteUserTokenCommand>
{
    public Task Handle(DeleteUserTokenCommand request, CancellationToken cancellationToken)
    {
        userTokenRepository.Remove(request.UserToken);

        return Task.CompletedTask;
    }
}
