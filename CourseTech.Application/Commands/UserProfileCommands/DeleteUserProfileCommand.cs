using CourseTech.Domain.Entities.UserRelated;
using MediatR;

namespace CourseTech.Application.Commands.UserProfileCommands;

/// <summary>
/// Удаление профиля пользователя.
/// </summary>
/// <param name="UserProfile"></param>
public record DeleteUserProfileCommand(UserProfile UserProfile) : IRequest;
