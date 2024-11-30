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
    /// Получение пользователя с его токеном и ролями по его логину.
    /// </summary>
    /// <param name="Login"></param>
    public record GetUserWithTokenAndRolesByLoginQuery(string Login) : IRequest<User>;
}
