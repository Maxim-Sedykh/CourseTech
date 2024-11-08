using AutoMapper;
using CourseTech.Application.Queries.RoleQueries;
using CourseTech.Domain.Dto.Role;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Extensions;
using CourseTech.Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CourseTech.Application.Handlers.RoleHandlers
{
    public class GetRoleDtosHandler(IBaseRepository<Role> roleRepository, IMapper mapper) : IRequestHandler<GetRoleDtosQuery, RoleDto[]>
    {
        public async Task<RoleDto[]> Handle(GetRoleDtosQuery request, CancellationToken cancellationToken)
        {
            return await roleRepository
                        .GetAll()
                        .AsProjected<Role, RoleDto>(mapper)
                        .ToArrayAsync(cancellationToken);
        }
    }
}
