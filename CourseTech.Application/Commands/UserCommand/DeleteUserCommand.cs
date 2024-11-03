using CourseTech.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Application.Commands.UserCommand
{
    /// <summary>
    /// Удаление пользователя.
    /// </summary>
    /// <param name="User"></param>
    public record DeleteUserCommand(User User) : IRequest;
}
