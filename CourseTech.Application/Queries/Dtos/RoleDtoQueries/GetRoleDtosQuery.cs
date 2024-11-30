using CourseTech.Domain.Dto.Role;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Application.Queries.Dtos.RoleDtoQueries
{
    /// <summary>
    /// Получение всех ролей в виде RoleDto
    /// </summary>
    public record GetRoleDtosQuery : IRequest<RoleDto[]>;
}
