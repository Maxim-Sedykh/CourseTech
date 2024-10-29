using AutoMapper;
using CourseTech.Application.Queries.LessonRecordQueries;
using CourseTech.Application.Queries.UserProfileQueries;
using CourseTech.Domain.Dto.FinalResult;
using CourseTech.Domain.Dto.LessonRecord;
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
    public class GetAnalysUserIdHandler(IBaseRepository<UserProfile> userProfileRepository, IMapper mapper) : IRequestHandler<GetAnalysByUserIdQuery, UserAnalysDto>
    {
        public async Task<UserAnalysDto> Handle(GetAnalysByUserIdQuery request, CancellationToken cancellationToken)
        {
            return await userProfileRepository.GetAll()
                        .Where(x => x.UserId == request.UserId)
                        .Select(x => mapper.Map<UserAnalysDto>(x))
                        .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
