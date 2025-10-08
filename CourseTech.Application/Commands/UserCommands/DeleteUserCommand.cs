using CourseTech.Domain.Entities.UserRelated;
using MediatR;

namespace CourseTech.Application.Commands.UserCommands;

/// <summary>
/// Удаление пользователя.
/// </summary>
/// <param name="User"></param>
public record DeleteUserCommand(User User) : IRequest;
