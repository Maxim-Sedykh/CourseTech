using MediatR;

namespace CourseTech.Application.Commands.UserProfileCommands;

/// <summary>
/// Создание профиля пользователя.
/// </summary>
/// <param name="UserId"></param>
/// <param name="Name"></param>
/// <param name="Surname"></param>
/// <param name="DateOfBirth"></param>
public record CreateUserProfileCommand(Guid UserId, DateTime DateOfBirth) : IRequest;
