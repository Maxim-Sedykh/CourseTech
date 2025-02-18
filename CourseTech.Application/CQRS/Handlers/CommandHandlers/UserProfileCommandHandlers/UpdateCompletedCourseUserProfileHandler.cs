using CourseTech.Application.CQRS.Commands.UserProfileCommands;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Interfaces.Repositories;
using MediatR;

namespace CourseTech.Application.CQRS.Handlers.CommandHandlers.UserProfileCommandHandlers
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
