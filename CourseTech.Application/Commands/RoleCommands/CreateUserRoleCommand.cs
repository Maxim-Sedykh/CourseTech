using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Application.Commands.RoleCommands
{
    /// <summary>
    /// Добавление роли для пользователя
    /// </summary>
    /// <param name="RoleId"></param>
    /// <param name="UserId"></param>
    public record CreateUserRoleCommand(long RoleId, Guid UserId) : IRequest;
}
