using CourseTech.Application.Commands.RoleCommands;
using CourseTech.DAL.Repositories;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Interfaces.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Application.Handlers.RoleHandlers
{
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
}
