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
    /// Обновление роли для пользователя.
    /// </summary>
    /// <param name="Role"></param>
    /// <param name="NewRoleName"></param>
    public record UpdateRoleCommand(Role Role, string NewRoleName) : IRequest;
}
