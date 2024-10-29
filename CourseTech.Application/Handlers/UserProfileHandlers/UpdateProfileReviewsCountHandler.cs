using AutoMapper;
using CourseTech.Application.Commands.Reviews;
using CourseTech.Application.Commands.UserProfileCommands;
using CourseTech.Application.Queries.UserQueries;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Interfaces.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Application.Handlers.UserProfileHandlers
{
    public class UpdateProfileReviewsCountHandler(IBaseRepository<UserProfile> userProfileRepository) : IRequestHandler<UpdateProfileReviewsCountCommand>
    {
        public Task Handle(UpdateProfileReviewsCountCommand request, CancellationToken cancellationToken)
        {
            request.UserProfile.CountOfReviews++;

            userProfileRepository.Update(request.UserProfile);

            return Task.CompletedTask;
        }
    }
}
