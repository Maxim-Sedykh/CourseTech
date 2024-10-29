using CourseTech.Application.Commands.UserProfileCommands;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Extensions;
using CourseTech.Domain.Interfaces.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Application.Handlers.UserProfileHandlers
{
    public class UpdateUserProfileHandler(IBaseRepository<UserProfile> userProfileRepository) : IRequestHandler<UpdateUserProfileCommand>
    {
        public async Task Handle(UpdateUserProfileCommand request, CancellationToken cancellationToken)
        {
            var profile = request.UserProfile;
            var updateUserProfileDto = request.UpdateUserProfileDto;

            var dateOfBirth = updateUserProfileDto.DateOfBirth;

            profile.Name = updateUserProfileDto.Name;
            profile.Surname = updateUserProfileDto.Surname;
            profile.DateOfBirth = dateOfBirth;
            profile.Age = dateOfBirth.GetYearsByDateToNow();

            userProfileRepository.Update(profile);

            await userProfileRepository.SaveChangesAsync(cancellationToken);
        }
    }
}
