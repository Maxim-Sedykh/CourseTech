using AutoMapper;
using CourseTech.Application.Queries.RoleQueries;
using CourseTech.DAL.Repositories;
using CourseTech.Domain.Dto.Role;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Application.Handlers.RoleHandlers
{
    public class GetRoleDtosHandler(IBaseRepository<Role> roleRepository, IMapper mapper) : IRequestHandler<GetRoleDtosQuery, RoleDto[]>
    {
        public async Task<RoleDto[]> Handle(GetRoleDtosQuery request, CancellationToken cancellationToken)
        {
            return await roleRepository
                        .GetAll()
                        .Select(x => mapper.Map<RoleDto>(x))
                        .ToArrayAsync(cancellationToken);
        }
    }
}
