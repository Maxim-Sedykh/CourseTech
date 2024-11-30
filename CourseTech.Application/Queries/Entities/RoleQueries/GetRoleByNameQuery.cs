using CourseTech.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Application.Queries.Entities.RoleQueries
{
    /// <summary>
    /// Получить роль по её названию.
    /// </summary>
    /// <param name="Name"></param>
    public record GetRoleByNameQuery(string Name) : IRequest<Role>;
}
