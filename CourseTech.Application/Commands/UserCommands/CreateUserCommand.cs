using CourseTech.Domain.Entities.UserRelated;
using MediatR;

namespace CourseTech.Application.Commands.UserCommands;

/// <summary>
/// Создание пользователя при регистрации.
/// </summary>
/// <param name="Login"></param>
/// <param name="Password"></param>
public record CreateUserCommand(string Login, string Password) : IRequest<User>;
