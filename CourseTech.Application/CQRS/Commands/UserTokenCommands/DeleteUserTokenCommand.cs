using CourseTech.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Application.CQRS.Commands.UserTokenCommands
{
    /// <summary>
    /// Удаление токена пользователя.
    /// </summary>
    /// <param name="UserToken"></param>
    public record DeleteUserTokenCommand(UserToken UserToken) : IRequest;
}
