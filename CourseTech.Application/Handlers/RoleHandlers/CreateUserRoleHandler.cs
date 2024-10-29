using CourseTech.Application.Commands.Reviews;
using CourseTech.Application.Commands.RoleCommands;
using CourseTech.DAL.Repositories;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Interfaces.Repositories;
using MediatR;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Application.Handlers.RoleHandlers
{
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
}
