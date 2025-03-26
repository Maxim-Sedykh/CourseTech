using CourseTech.Application.CQRS.Commands.UserCommand;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Interfaces.Helpers;
using CourseTech.Domain.Interfaces.Repositories;
using MediatR;

namespace CourseTech.Application.CQRS.Handlers.CommandHandlers.UserCommandHandlers;

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
