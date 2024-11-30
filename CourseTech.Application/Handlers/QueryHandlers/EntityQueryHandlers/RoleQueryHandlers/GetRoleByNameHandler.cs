using CourseTech.Application.Queries.Entities.RoleQueries;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CourseTech.Application.Handlers.QueryHandlers.EntityQueryHandlers.RoleQueryHandlers
{
    public class GetRoleByNameHandler(IBaseRepository<Role> roleRepository) : IRequestHandler<GetRoleByNameQuery, Role>
    {
        public async Task<Role> Handle(GetRoleByNameQuery request, CancellationToken cancellationToken)
        {
            return await roleRepository.GetAll().FirstOrDefaultAsync(x => x.Name == request.Name, cancellationToken);
        }
    }
}
