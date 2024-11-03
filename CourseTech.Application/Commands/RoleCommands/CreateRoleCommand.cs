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
    /// Создание роли.
    /// </summary>
    /// <param name="RoleName"></param>
    public record CreateRoleCommand(string RoleName) : IRequest;
}
