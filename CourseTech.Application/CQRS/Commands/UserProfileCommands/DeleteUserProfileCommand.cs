using CourseTech.Domain.Entities;
using MediatR;

namespace CourseTech.Application.CQRS.Commands.UserProfileCommands;

/// <summary>
/// Удаление профиля пользователя.
/// </summary>
/// <param name="UserProfile"></param>
public record DeleteUserProfileCommand(UserProfile UserProfile) : IRequest;
