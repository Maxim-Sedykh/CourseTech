﻿using CourseTech.Application.Commands.UserProfileCommands;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Interfaces.Repositories;
using MediatR;

namespace CourseTech.Application.Handlers.CommandHandlers.UserProfileCommandHandlers
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