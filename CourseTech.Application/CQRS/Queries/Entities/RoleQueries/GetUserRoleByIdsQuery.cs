using CourseTech.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Application.CQRS.Queries.Entities.RoleQueries
{
    /// <summary>
    /// Получение сущности "UserRole" по его идентификаторам.
    /// </summary>
    /// <param name="RoleId"></param>
    /// <param name="UserId"></param>
    public record GetUserRoleByIdsQuery(long RoleId, Guid UserId) : IRequest<UserRole>;
}
