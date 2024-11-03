using CourseTech.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Application.Queries.RoleQueries
{
    /// <summary>
    /// Получение роли по её идентификатору.
    /// </summary>
    /// <param name="RoleId"></param>
    public record GetRoleByIdQuery(long RoleId) : IRequest<Role>;
}
