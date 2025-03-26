using CourseTech.Application.CQRS.Commands.RoleCommands;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Interfaces.Repositories;
using MediatR;

namespace CourseTech.Application.CQRS.Handlers.CommandHandlers.RoleCommandHandlers;

public class CreateRoleHandler(IBaseRepository<Role> roleRepository) : IRequestHandler<CreateRoleCommand>
{
    public async Task Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        var role = new Role()
        {
            Name = request.RoleName,
        };

        await roleRepository.CreateAsync(role);
        await roleRepository.SaveChangesAsync(cancellationToken);
    }
}
