using CourseTech.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Application.Queries.Entities.UserQueries
{
    /// <summary>
    /// Получение пользователя с ролями по логину пользователя.
    /// </summary>
    /// <param name="Login"></param>
    public record GetUserWithRolesByLoginQuery(string Login) : IRequest<User>;
}
