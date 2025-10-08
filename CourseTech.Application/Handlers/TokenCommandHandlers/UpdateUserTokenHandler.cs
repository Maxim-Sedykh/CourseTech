using CourseTech.Application.Commands.UserTokenCommands;
using CourseTech.Domain.Interfaces.Repositories;
using MediatR;

namespace CourseTech.Application.Handlers.TokenCommandHandlers;

public class UpdateUserTokenHandler(IUserTokenRepository userTokenRepository) : IRequestHandler<UpdateUserTokenCommand>
{
    public async Task Handle(UpdateUserTokenCommand request, CancellationToken cancellationToken)
    {
        var userToken = request.UserToken;

        userToken.RefreshToken = request.RefreshToken;
        userToken.RefreshTokenExpireTime = DateTime.UtcNow.AddDays(7);

        userTokenRepository.Update(userToken);

        await userTokenRepository.SaveChangesAsync(cancellationToken);
    }
}
