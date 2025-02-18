using CourseTech.Application.CQRS.Commands.UserCommand;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Interfaces.Repositories;
using MediatR;

namespace CourseTech.Application.CQRS.Handlers.CommandHandlers.UserCommandHandlers
{
    public class DeleteUserHandler(IBaseRepository<User> userRepository) : IRequestHandler<DeleteUserCommand>
    {
        public Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            userRepository.Remove(request.User);

            return Task.CompletedTask;
        }
    }
}
