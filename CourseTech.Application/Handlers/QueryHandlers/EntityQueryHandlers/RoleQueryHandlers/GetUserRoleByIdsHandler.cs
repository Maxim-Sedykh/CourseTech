using CourseTech.Application.Queries.Entities.RoleQueries;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CourseTech.Application.Handlers.QueryHandlers.EntityQueryHandlers.RoleQueryHandlers
{
    public class GetUserRoleByIdsHandler(IBaseRepository<UserRole> userRoleRepository) : IRequestHandler<GetUserRoleByIdsQuery, UserRole>
    {
        public async Task<UserRole> Handle(GetUserRoleByIdsQuery request, CancellationToken cancellationToken)
        {
            return await userRoleRepository.GetAll()
                .FirstOrDefaultAsync(x => x.UserId == request.UserId && x.RoleId == request.RoleId, cancellationToken);
        }
    }
}
