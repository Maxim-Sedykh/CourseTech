using CourseTech.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Application.Queries.Entities.UserTokenQueries
{
    /// <summary>
    /// Получение токена пользователя по идентификатору пользователя.
    /// </summary>
    /// <param name="UserId"></param>
    public record GetUserTokenByUserIdQuery(Guid UserId) : IRequest<UserToken>;
}
