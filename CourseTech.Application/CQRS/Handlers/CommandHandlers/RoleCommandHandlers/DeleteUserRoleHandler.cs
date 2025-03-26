using CourseTech.Application.CQRS.Commands.RoleCommands;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Interfaces.Repositories;
using MediatR;

namespace CourseTech.Application.CQRS.Handlers.CommandHandlers.RoleCommandHandlers;

public class DeleteUserRoleHandler(IBaseRepository<UserRole> userRoleRepository) : IRequestHandler<DeleteUserRoleCommand>
{
    public Task Handle(DeleteUserRoleCommand request, CancellationToken cancellationToken)
    {
        userRoleRepository.Remove(request.UserRole);

        return Task.CompletedTask;
    }
}
