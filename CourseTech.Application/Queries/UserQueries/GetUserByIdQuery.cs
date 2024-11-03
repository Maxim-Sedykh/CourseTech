using CourseTech.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Application.Queries.UserQueries
{
    /// <summary>
    /// Получить пользователя по идентификатору.
    /// </summary>
    /// <param name="UserId"></param>
    public record GetUserByIdQuery(Guid UserId) : IRequest<User>;
}
