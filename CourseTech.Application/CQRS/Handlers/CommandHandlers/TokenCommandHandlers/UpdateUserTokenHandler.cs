using CourseTech.Application.CQRS.Commands.UserTokenCommands;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Interfaces.Repositories;
using MediatR;

namespace CourseTech.Application.CQRS.Handlers.CommandHandlers.TokenCommandHandlers;

public class UpdateUserTokenHandler(IBaseRepository<UserToken> userTokenRepository) : IRequestHandler<UpdateUserTokenCommand>
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
