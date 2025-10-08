using CourseTech.Application.Commands.UserProfileCommands;
using CourseTech.Domain.Interfaces.Repositories;
using MediatR;

namespace CourseTech.Application.Handlers.UserProfileCommandHandlers;

public class DeleteUserProfileHandler(IUserProfileRepository userProfileRepository) : IRequestHandler<DeleteUserProfileCommand>
{
    public Task Handle(DeleteUserProfileCommand request, CancellationToken cancellationToken)
    {
        userProfileRepository.Remove(request.UserProfile);

        return Task.CompletedTask;
    }
}
