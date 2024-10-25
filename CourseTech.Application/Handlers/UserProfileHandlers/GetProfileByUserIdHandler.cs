using AutoMapper;
using CourseTech.Application.Queries.Reviews;
using CourseTech.Application.Queries.UserQueries;
using CourseTech.Domain.Dto.Review;
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
    public class GetProfileByUserIdHandler(IBaseRepository<UserProfile> userProfileRepository) : IRequestHandler<GetProfileByUserIdQuery, UserProfile>
    {
        public async Task<UserProfile> Handle(GetProfileByUserIdQuery request, CancellationToken cancellationToken)
        {
            return await userProfileRepository.GetAll().FirstOrDefaultAsync(x => x.UserId == request.UserId);
        }
    }
}
