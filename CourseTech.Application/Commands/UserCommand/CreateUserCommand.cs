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
    /// Создание пользователя при регистрации.
    /// </summary>
    /// <param name="Login"></param>
    /// <param name="Password"></param>
    public record CreateUserCommand(string Login, string Password) : IRequest<User>;
}
