using CourseTech.Application.Commands.Reviews;
using CourseTech.Application.Commands.UserCommand;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Interfaces.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Application.Handlers.CommandHandlers.UserCommandHandlers
{
    public class DeleteUserHandler(IBaseRepository<User> userRepository) : IRequestHandler<DeleteUserCommand>
    {
        public Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            userRepository.Remove(request.User);

            return Task.CompletedTask;
        }
    }
}
