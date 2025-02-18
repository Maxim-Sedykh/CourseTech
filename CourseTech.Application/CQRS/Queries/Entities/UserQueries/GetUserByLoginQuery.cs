using CourseTech.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Application.CQRS.Queries.Entities.UserQueries
{
    /// <summary>
    /// Получение пользователя по его логину.
    /// </summary>
    /// <param name="Login"></param>
    public record GetUserByLoginQuery(string Login) : IRequest<User>;
}
