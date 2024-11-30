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
    /// Получение пользователя с профилем по идентификатору пользователя.
    /// </summary>
    /// <param name="UserId"></param>
    public record GetUserWithProfileByUserIdQuery(Guid UserId) : IRequest<User>;
}
