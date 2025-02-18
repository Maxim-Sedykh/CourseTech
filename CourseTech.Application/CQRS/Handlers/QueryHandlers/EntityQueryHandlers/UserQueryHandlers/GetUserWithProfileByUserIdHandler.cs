using CourseTech.Application.CQRS.Queries.Entities.UserQueries;
using CourseTech.Domain.Dto.User;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Application.CQRS.Handlers.QueryHandlers.EntityQueryHandlers.UserQueryHandlers
{
    public class GetUserWithProfileByUserIdHandler(IBaseRepository<User> userRepository) : IRequestHandler<GetUserWithProfileByUserIdQuery, User>
    {
        public async Task<User> Handle(GetUserWithProfileByUserIdQuery request, CancellationToken cancellationToken)
        {
            return await userRepository.GetAll()
                .Include(x => x.UserProfile)
                .FirstOrDefaultAsync(x => x.Id == request.UserId, cancellationToken);
        }
    }
}
