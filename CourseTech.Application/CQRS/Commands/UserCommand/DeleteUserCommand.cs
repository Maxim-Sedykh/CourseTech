using CourseTech.Domain.Entities;
using MediatR;

namespace CourseTech.Application.CQRS.Commands.UserCommand;

/// <summary>
/// Удаление пользователя.
/// </summary>
/// <param name="User"></param>
public record DeleteUserCommand(User User) : IRequest;
