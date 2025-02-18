using CourseTech.Application.CQRS.Commands.UserProfileCommands;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Interfaces.Repositories;
using MediatR;

namespace CourseTech.Application.CQRS.Handlers.CommandHandlers.UserProfileCommandHandlers
{
    public class UpdateProfileCompletingLessonHandler(IBaseRepository<UserProfile> userProfileRepository) : IRequestHandler<UpdateProfileCompletingLessonCommand>
    {
        public Task Handle(UpdateProfileCompletingLessonCommand request, CancellationToken cancellationToken)
        {
            var profile = request.UserProfile;

            profile.CurrentGrade += request.UserGrade;
            profile.LessonsCompleted++;

            userProfileRepository.Update(profile);

            return Task.CompletedTask;
        }
    }
}
