using CourseTech.Application.CQRS.Commands.RoleCommands;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Interfaces.Repositories;
using MediatR;

namespace CourseTech.Application.CQRS.Handlers.CommandHandlers.RoleCommandHandlers;

public class CreateUserRoleHandler(IBaseRepository<UserRole> userRoleRepository) : IRequestHandler<CreateUserRoleCommand>
{
    public async Task Handle(CreateUserRoleCommand request, CancellationToken cancellationToken)
    {
        var userRole = new UserRole()
        {
            RoleId = request.RoleId,
            UserId = request.UserId
        };

        await userRoleRepository.CreateAsync(userRole);
    }
}
