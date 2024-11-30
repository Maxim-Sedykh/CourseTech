using AutoMapper;
using CourseTech.Application.Commands.UserCommand;
using CourseTech.Application.Commands.UserTokenCommands;
using CourseTech.DAL.Repositories;
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
    public class UpdateUserHandler(IBaseRepository<User> userRepository, IMapper mapper) : IRequestHandler<UpdateUserCommand>
    {
        public async Task Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = request.User;

            mapper.Map(request.UpdateUserDto, user);

            userRepository.Update(user);
            await userRepository.SaveChangesAsync(cancellationToken);
        }
    }
}
