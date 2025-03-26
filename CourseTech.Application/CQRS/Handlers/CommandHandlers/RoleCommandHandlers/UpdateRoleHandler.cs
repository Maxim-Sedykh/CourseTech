using CourseTech.Application.CQRS.Commands.RoleCommands;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Interfaces.Repositories;
using MediatR;

namespace CourseTech.Application.CQRS.Handlers.CommandHandlers.RoleCommandHandlers;

public class UpdateRoleHandler(IBaseRepository<Role> roleRepository) : IRequestHandler<UpdateRoleCommand>
{
    public async Task Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
    {
        var role = request.Role;

        role.Name = request.NewRoleName;

        roleRepository.Update(role);
        await roleRepository.SaveChangesAsync(cancellationToken);
    }
}
