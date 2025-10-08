using CourseTech.Application.Commands.UserProfileCommands;
using CourseTech.Domain.Extensions;
using CourseTech.Domain.Interfaces.Repositories;
using MediatR;

namespace CourseTech.Application.Handlers.UserProfileCommandHandlers;

public class UpdateUserProfileHandler(IUserProfileRepository userProfileRepository) : IRequestHandler<UpdateUserProfileCommand>
{
    public async Task Handle(UpdateUserProfileCommand request, CancellationToken cancellationToken)
    {
        var profile = request.UserProfile;
        var updateUserProfileDto = request.UpdateUserProfileDto;

        var dateOfBirth = updateUserProfileDto.DateOfBirth;

        profile.DateOfBirth = dateOfBirth;
        profile.Age = dateOfBirth.GetYearsByDateToNow();

        userProfileRepository.Update(profile);

        await userProfileRepository.SaveChangesAsync(cancellationToken);
    }
}
