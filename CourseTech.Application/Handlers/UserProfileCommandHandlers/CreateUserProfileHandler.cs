using CourseTech.Application.Commands.UserProfileCommands;
using CourseTech.Domain.Entities.UserRelated;
using CourseTech.Domain.Extensions;
using CourseTech.Domain.Interfaces.Repositories;
using MediatR;

namespace CourseTech.Application.Handlers.UserProfileCommandHandlers;

public class CreateUserProfileHandler(IUserProfileRepository userProfileRepository) : IRequestHandler<CreateUserProfileCommand>
{
    public async Task Handle(CreateUserProfileCommand request, CancellationToken cancellationToken)
    {
        var dateOfBirth = request.DateOfBirth;

        var userProfile = new UserProfile()
        {
            UserId = request.UserId,
            Age = dateOfBirth.GetYearsByDateToNow(),
            DateOfBirth = dateOfBirth,
            LessonsCompleted = 0
        };

        await userProfileRepository.CreateAsync(userProfile);
    }
}
