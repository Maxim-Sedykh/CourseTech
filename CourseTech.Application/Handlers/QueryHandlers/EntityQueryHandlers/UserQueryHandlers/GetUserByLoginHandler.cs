using CourseTech.Application.Queries.Entities.UserQueries;
using CourseTech.DAL.Repositories;
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
    public class GetUserByLoginHandler(IBaseRepository<User> userRepository) : IRequestHandler<GetUserByLoginQuery, User>
    {
        public async Task<User> Handle(GetUserByLoginQuery request, CancellationToken cancellationToken)
        {
            return await userRepository.GetAll().FirstOrDefaultAsync(x => x.Login == request.Login, cancellationToken);
        }
    }
}
