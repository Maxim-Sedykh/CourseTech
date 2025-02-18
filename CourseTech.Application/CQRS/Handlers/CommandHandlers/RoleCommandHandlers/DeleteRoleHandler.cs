using CourseTech.Application.CQRS.Commands.RoleCommands;
using CourseTech.DAL.Repositories;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Interfaces.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Application.CQRS.Handlers.CommandHandlers.RoleCommandHandlers
{
    public class DeleteRoleHandler(IBaseRepository<Role> roleRepository) : IRequestHandler<DeleteRoleCommand>
    {
        public async Task Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
        {
            roleRepository.Remove(request.Role);
            await roleRepository.SaveChangesAsync(cancellationToken);
        }
    }
}
