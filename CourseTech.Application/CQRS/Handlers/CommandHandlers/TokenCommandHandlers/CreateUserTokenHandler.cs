using CourseTech.Application.CQRS.Commands.UserTokenCommands;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Interfaces.Databases.Repositories;
using MediatR;

namespace CourseTech.Application.CQRS.Handlers.CommandHandlers.TokenCommandHandlers;

public class CreateUserTokenHandler(IBaseRepository<UserToken> userTokenRepository) : IRequestHandler<CreateUserTokenCommand>
{
    public async Task Handle(CreateUserTokenCommand request, CancellationToken cancellationToken)
    {
        var userToken = new UserToken()
        {
            UserId = request.UserId,
            RefreshToken = request.RefreshToken,
            RefreshTokenExpireTime = DateTime.UtcNow.AddDays(request.RefreshTokenValidityInDays)
        };

        await userTokenRepository.CreateAsync(userToken);

        await userTokenRepository.SaveChangesAsync(cancellationToken);
    }
}
