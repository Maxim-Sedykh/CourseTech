using CourseTech.Application.Commands.UserCommand;
using CourseTech.Application.CQRS.Commands.UserProfileCommands;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Interfaces.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Application.CQRS.Handlers.CommandHandlers.UserProfileCommandHandlers
{
    public class DeleteUserProfileHandler(IBaseRepository<UserProfile> userProfileRepository) : IRequestHandler<DeleteUserProfileCommand>
    {
        public Task Handle(DeleteUserProfileCommand request, CancellationToken cancellationToken)
        {
            userProfileRepository.Remove(request.UserProfile);

            return Task.CompletedTask;
        }
    }
}
