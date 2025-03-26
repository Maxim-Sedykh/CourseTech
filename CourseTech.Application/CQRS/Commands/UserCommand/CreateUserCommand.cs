using CourseTech.Domain.Entities;
using MediatR;

namespace CourseTech.Application.CQRS.Commands.UserCommand;

/// <summary>
/// Создание пользователя при регистрации.
/// </summary>
/// <param name="Login"></param>
/// <param name="Password"></param>
public record CreateUserCommand(string Login, string Password) : IRequest<User>;
