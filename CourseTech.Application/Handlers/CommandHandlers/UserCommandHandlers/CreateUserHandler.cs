using CourseTech.Application.Commands.Reviews;
using CourseTech.Application.Commands.UserCommand;
using CourseTech.DAL.Auth;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Interfaces.Helpers;
using CourseTech.Domain.Interfaces.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Application.Handlers.CommandHandlers.UserCommandHandlers
{
    public class CreateUserHandler(IBaseRepository<User> userRepository, IPasswordHasher passwordHasher) : IRequestHandler<CreateUserCommand, User>
    {
        public async Task<User> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var user = new User()
            {
                Login = request.Login,
                Password = passwordHasher.Hash(request.Password),
            };

            await userRepository.CreateAsync(user);

            await userRepository.SaveChangesAsync(cancellationToken);

            return user;
        }
    }
}
