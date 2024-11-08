using AutoMapper;
using CourseTech.Application.Queries.UserProfileQueries;
using CourseTech.Domain.Dto.FinalResult;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Extensions;
using CourseTech.Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CourseTech.Application.Handlers.UserProfileHandlers
{
    public class GetAnalysUserIdHandler(IBaseRepository<UserProfile> userProfileRepository, IMapper mapper) : IRequestHandler<GetAnalysByUserIdQuery, UserAnalysDto>
    {
        public async Task<UserAnalysDto> Handle(GetAnalysByUserIdQuery request, CancellationToken cancellationToken)
        {
            return await userProfileRepository.GetAll()
                        .Where(x => x.UserId == request.UserId)
                        .AsProjected<UserProfile, UserAnalysDto>(mapper)
                        .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
