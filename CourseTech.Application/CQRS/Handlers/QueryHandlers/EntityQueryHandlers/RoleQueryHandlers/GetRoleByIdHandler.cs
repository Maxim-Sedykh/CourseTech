using CourseTech.Application.CQRS.Queries.Entities.RoleQueries;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CourseTech.Application.CQRS.Handlers.QueryHandlers.EntityQueryHandlers.RoleQueryHandlers;

public class GetRoleByIdHandler(IBaseRepository<Role> roleRepository) : IRequestHandler<GetRoleByIdQuery, Role>
{
    public async Task<Role> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
    {
        return await roleRepository.GetAll().FirstOrDefaultAsync(x => x.Id == request.RoleId, cancellationToken);
    }
}
