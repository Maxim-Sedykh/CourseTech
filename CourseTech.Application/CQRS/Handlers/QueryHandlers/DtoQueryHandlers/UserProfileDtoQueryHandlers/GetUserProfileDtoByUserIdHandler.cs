using AutoMapper;
using CourseTech.Application.CQRS.Queries.Dtos.UserProfileDtoQuery;
using CourseTech.Domain.Dto.UserProfile;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Extensions;
using CourseTech.Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CourseTech.Application.CQRS.Handlers.QueryHandlers.DtoQueryHandlers.UserProfileDtoQueryHandlers;

public class GetUserProfileDtoByUserIdHandler(IBaseRepository<UserProfile> userProfileRepository, IMapper mapper) : IRequestHandler<GetUserProfileDtoByUserIdQuery, UserProfileDto>
{
    public async Task<UserProfileDto> Handle(GetUserProfileDtoByUserIdQuery request, CancellationToken cancellationToken)
    {
        return await userProfileRepository.GetAll()
                    .Include(x => x.User)
                    .Where(x => x.UserId == request.UserId)
                    .AsProjected<UserProfile, UserProfileDto>(mapper)
                    .FirstOrDefaultAsync(cancellationToken);
    }
}
