using CourseTech.Application.Commands.UserTokenCommands;
using CourseTech.Domain.Entities.UserRelated;
using CourseTech.Domain.Interfaces.Repositories;
using MediatR;

namespace CourseTech.Application.Handlers.TokenCommandHandlers;

public class CreateUserTokenHandler(IUserTokenRepository userTokenRepository) : IRequestHandler<CreateUserTokenCommand>
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
