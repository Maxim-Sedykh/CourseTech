using CourseTech.Application.CQRS.Commands.UserProfileCommands;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Extensions;
using CourseTech.Domain.Interfaces.Repositories;
using MediatR;

namespace CourseTech.Application.CQRS.Handlers.CommandHandlers.UserProfileCommandHandlers;

public class UpdateUserProfileHandler(IBaseRepository<UserProfile> userProfileRepository) : IRequestHandler<UpdateUserProfileCommand>
{
    public async Task Handle(UpdateUserProfileCommand request, CancellationToken cancellationToken)
    {
        var profile = request.UserProfile;
        var updateUserProfileDto = request.UpdateUserProfileDto;

        var dateOfBirth = updateUserProfileDto.DateOfBirth;

        profile.Name = updateUserProfileDto.UserName;
        profile.Surname = updateUserProfileDto.Surname;
        profile.DateOfBirth = dateOfBirth;
        profile.Age = dateOfBirth.GetYearsByDateToNow();

        userProfileRepository.Update(profile);

        await userProfileRepository.SaveChangesAsync(cancellationToken);
    }
}
