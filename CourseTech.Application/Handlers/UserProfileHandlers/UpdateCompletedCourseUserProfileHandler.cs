using AutoMapper;
using CourseTech.Application.Commands.UserProfileCommands;
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
    public class UpdateCompletedCourseUserProfileHandler(IBaseRepository<UserProfile> userProfileRepository) : IRequestHandler<UpdateCompletedCourseUserProfileCommand>
    {
        public async Task Handle(UpdateCompletedCourseUserProfileCommand request, CancellationToken cancellationToken)
        {
            var profile = request.UserProfile;

            profile.Analys = request.Analys;
            profile.IsExamCompleted = true;

            userProfileRepository.Update(profile);
            await userProfileRepository.SaveChangesAsync(cancellationToken);
        }
    }
}
