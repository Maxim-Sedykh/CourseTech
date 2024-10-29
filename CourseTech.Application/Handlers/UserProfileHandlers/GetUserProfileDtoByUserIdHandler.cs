using AutoMapper;
using CourseTech.Application.Queries.UserProfileQueries;
using CourseTech.Application.Queries.UserQueries;
using CourseTech.Domain.Dto.UserProfile;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Application.Handlers.UserProfileHandlers
{
    public class GetUserProfileDtoByUserIdHandler(IBaseRepository<UserProfile> userProfileRepository, IMapper mapper) : IRequestHandler<GetUserProfileDtoByUserIdQuery, UserProfileDto>
    {
        public async Task<UserProfileDto> Handle(GetUserProfileDtoByUserIdQuery request, CancellationToken cancellationToken)
        {
            return await userProfileRepository.GetAll()
                        .Include(x => x.User)
                        .Where(x => x.UserId == request.UserId)
                        .Select(x => mapper.Map<UserProfileDto>(x))
                        .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
