using AutoMapper;
using CourseTech.Application.CQRS.Queries.Dtos.RoleDtoQueries;
using CourseTech.Domain.Dto.Role;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Extensions;
using CourseTech.Domain.Interfaces.Databases.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CourseTech.Application.CQRS.Handlers.QueryHandlers.DtoQueryHandlers.RoleDtoQueryHandlers;

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
