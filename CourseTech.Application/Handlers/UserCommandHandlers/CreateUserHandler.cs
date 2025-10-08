using CourseTech.Application.Commands.UserCommands;
using CourseTech.Domain.Entities.UserRelated;
using CourseTech.Domain.Interfaces.Helpers;
using CourseTech.Domain.Interfaces.Repositories;
using MediatR;

namespace CourseTech.Application.Handlers.UserCommandHandlers;

public class CreateUserHandler(IUserRepository userRepository, IPasswordHasher passwordHasher) : IRequestHandler<CreateUserCommand, User>
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
