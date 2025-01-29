﻿using AutoMapper;
using CourseTech.Application.Queries.Dtos.UserProfileDtoQuery;
using CourseTech.Domain.Dto.CourseResult;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Extensions;
using CourseTech.Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CourseTech.Application.Handlers.QueryHandlers.DtoQueryHandlers.UserProfileDtoQueryHandlers
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
