using AutoMapper;
using CourseTech.Application.CQRS.Commands.UserCommand;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Interfaces.Databases.Repositories;
using MediatR;

namespace CourseTech.Application.CQRS.Handlers.CommandHandlers.UserCommandHandlers;

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
