using CourseTech.Application.Commands.RoleCommands;
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
    public class DeleteUserRoleHandler(IBaseRepository<UserRole> userRoleRepository) : IRequestHandler<DeleteUserRoleCommand>
    {
        public Task Handle(DeleteUserRoleCommand request, CancellationToken cancellationToken)
        {
            userRoleRepository.Remove(request.UserRole);
            
            return Task.CompletedTask;
        }
    }
}
