using CourseTech.Application.CQRS.Commands.RoleCommands;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Interfaces.Databases.Repositories;
using MediatR;

namespace CourseTech.Application.CQRS.Handlers.CommandHandlers.RoleCommandHandlers;

public class DeleteRoleHandler(IBaseRepository<Role> roleRepository) : IRequestHandler<DeleteRoleCommand>
{
    public async Task Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
    {
        roleRepository.Remove(request.Role);
        await roleRepository.SaveChangesAsync(cancellationToken);
    }
}
