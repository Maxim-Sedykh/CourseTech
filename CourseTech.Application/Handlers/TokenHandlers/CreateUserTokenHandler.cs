﻿using CourseTech.Application.Commands.Reviews;
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

namespace CourseTech.Application.Handlers.TokenHandlers
{
    public class CreateUserTokenHandler(IBaseRepository<UserToken> userTokenRepository) : IRequestHandler<CreateUserTokenCommand>
    {
        public async Task Handle(CreateUserTokenCommand request, CancellationToken cancellationToken)
        {
            var userToken = new UserToken()
            {
                UserId = request.UserId,
                RefreshToken = request.RefreshToken,
                RefreshTokenExpireTime = DateTime.UtcNow.AddDays(7) // To Do hardcoude beach
            };

            await userTokenRepository.CreateAsync(userToken);

            await userTokenRepository.SaveChangesAsync(cancellationToken);
        }
    }
}