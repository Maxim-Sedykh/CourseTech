using CourseTech.Application.Commands.UserCommands;
using CourseTech.Domain.Interfaces.Repositories;
using MediatR;

namespace CourseTech.Application.Handlers.UserCommandHandlers;

public class DeleteUserHandler(IUserRepository userRepository) : IRequestHandler<DeleteUserCommand>
{
    public Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        userRepository.Remove(request.User);

        return Task.CompletedTask;
    }
}
