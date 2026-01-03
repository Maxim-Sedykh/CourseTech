using CourseTech.Application.CQRS.Queries.Entities.UserProfileQueries;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Interfaces.Databases.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CourseTech.Application.CQRS.Handlers.QueryHandlers.EntityQueryHandlers.UserProfileQueryHandlers;

public class GetProfileByUserIdHandler(IBaseRepository<UserProfile> userProfileRepository) : IRequestHandler<GetProfileByUserIdQuery, UserProfile>
{
    public async Task<UserProfile> Handle(GetProfileByUserIdQuery request, CancellationToken cancellationToken)
    {
        return await userProfileRepository.GetAll().FirstOrDefaultAsync(x => x.UserId == request.UserId, cancellationToken);
    }
}
