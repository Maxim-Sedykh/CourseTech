using CourseTech.Application.Queries.Reviews;
using CourseTech.Application.Queries.UserTokenQueries;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Application.Handlers.TokenHandlers
{
    public class GetUserTokenByUserIdHandler(IBaseRepository<UserToken> userTokenRepository) : IRequestHandler<GetUserTokenByUserIdQuery, UserToken>
    {
        public async Task<UserToken> Handle(GetUserTokenByUserIdQuery request, CancellationToken cancellationToken)
        {
            return await userTokenRepository.GetAll()
                .FirstOrDefaultAsync(x => x.UserId == request.UserId, cancellationToken);
        }
    }
}
