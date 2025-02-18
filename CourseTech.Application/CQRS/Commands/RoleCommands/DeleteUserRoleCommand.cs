using CourseTech.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Application.CQRS.Commands.RoleCommands
{
    /// <summary>
    /// Удаление роли для пользователя.
    /// </summary>
    /// <param name="UserRole"></param>
    public record DeleteUserRoleCommand(UserRole UserRole) : IRequest;
}
