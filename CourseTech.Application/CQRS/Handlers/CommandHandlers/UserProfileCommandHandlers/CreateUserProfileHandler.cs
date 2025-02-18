using CourseTech.Application.CQRS.Commands.UserProfileCommands;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Extensions;
using CourseTech.Domain.Interfaces.Repositories;
using MediatR;

namespace CourseTech.Application.CQRS.Handlers.CommandHandlers.UserProfileCommandHandlers
{
    public class CreateUserProfileHandler(IBaseRepository<UserProfile> userProfileRepository) : IRequestHandler<CreateUserProfileCommand>
    {
        public async Task Handle(CreateUserProfileCommand request, CancellationToken cancellationToken)
        {
            var dateOfBirth = request.DateOfBirth;

            UserProfile userProfile = new UserProfile()
            {
                UserId = request.UserId,
                IsEditAble = true,
                Name = request.Name,
                Surname = request.Surname,
                Age = dateOfBirth.GetYearsByDateToNow(),
                DateOfBirth = dateOfBirth,
                IsExamCompleted = false,
                CurrentGrade = 0,
                LessonsCompleted = 0,
                CountOfReviews = 0
            };

            await userProfileRepository.CreateAsync(userProfile);
        }
    }
}
