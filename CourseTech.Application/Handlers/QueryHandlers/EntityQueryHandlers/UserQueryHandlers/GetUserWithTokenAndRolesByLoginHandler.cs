using CourseTech.Application.Queries.Entities.UserQueries;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Application.Handlers.QueryHandlers.EntityQueryHandlers.UserQueryHandlers
{
    public class GetUserWithTokenAndRolesByLoginHandler(IBaseRepository<User> userRepository) : IRequestHandler<GetUserWithTokenAndRolesByLoginQuery, User>
    {
        public async Task<User> Handle(GetUserWithTokenAndRolesByLoginQuery request, CancellationToken cancellationToken)
        {
            return await userRepository.GetAll()
                .Include(x => x.UserToken)
                .Include(x => x.Roles)
                .FirstOrDefaultAsync(x => x.Login == request.Login, cancellationToken);
        }
    }
}
