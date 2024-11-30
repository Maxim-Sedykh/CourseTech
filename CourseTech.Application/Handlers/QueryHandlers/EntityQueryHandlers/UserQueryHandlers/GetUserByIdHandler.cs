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
    public class GetUserByIdHandler(IBaseRepository<User> userRepository) : IRequestHandler<GetUserByIdQuery, User>
    {
        public async Task<User> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            return await userRepository.GetAll()
                .FirstOrDefaultAsync(x => x.Id == request.UserId, cancellationToken);
        }
    }
}
