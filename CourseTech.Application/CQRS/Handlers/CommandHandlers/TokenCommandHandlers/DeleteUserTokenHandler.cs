using CourseTech.Application.Commands.UserProfileCommands;
using CourseTech.Application.CQRS.Commands.UserTokenCommands;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Interfaces.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Application.CQRS.Handlers.CommandHandlers.TokenCommandHandlers
{
    public class DeleteUserTokenHandler(IBaseRepository<UserToken> userTokenRepository) : IRequestHandler<DeleteUserTokenCommand>
    {
        public Task Handle(DeleteUserTokenCommand request, CancellationToken cancellationToken)
        {
            userTokenRepository.Remove(request.UserToken);

            return Task.CompletedTask;
        }
    }
}
