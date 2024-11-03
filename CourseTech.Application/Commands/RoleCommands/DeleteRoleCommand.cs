using CourseTech.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Application.Commands.RoleCommands
{
    /// <summary>
    /// Удаление роли
    /// </summary>
    /// <param name="Role"></param>
    public record DeleteRoleCommand(Role Role) : IRequest;
}
