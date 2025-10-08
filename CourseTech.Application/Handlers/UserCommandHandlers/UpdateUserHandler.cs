using AutoMapper;
using CourseTech.Application.Commands.UserCommands;
using CourseTech.Domain.Interfaces.Repositories;
using MediatR;

namespace CourseTech.Application.Handlers.UserCommandHandlers;

public class UpdateUserHandler(IUserRepository userRepository, IMapper mapper) : IRequestHandler<UpdateUserCommand>
{
    public async Task Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = request.User;

        mapper.Map(request.UpdateUserDto, user);

        userRepository.Update(user);
        await userRepository.SaveChangesAsync(cancellationToken);
    }
}
