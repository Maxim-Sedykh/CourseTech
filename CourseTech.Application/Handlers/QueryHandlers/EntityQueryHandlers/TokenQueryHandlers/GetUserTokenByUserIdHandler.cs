using CourseTech.Application.Queries.Entities.UserTokenQueries;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CourseTech.Application.Handlers.QueryHandlers.EntityQueryHandlers.TokenQueryHandlers
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
