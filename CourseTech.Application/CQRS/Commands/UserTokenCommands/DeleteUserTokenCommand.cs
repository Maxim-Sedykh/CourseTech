﻿using CourseTech.Domain.Entities;
using MediatR;

namespace CourseTech.Application.CQRS.Commands.UserTokenCommands;

/// <summary>
/// Удаление токена пользователя.
/// </summary>
/// <param name="UserToken"></param>
public record DeleteUserTokenCommand(UserToken UserToken) : IRequest;
