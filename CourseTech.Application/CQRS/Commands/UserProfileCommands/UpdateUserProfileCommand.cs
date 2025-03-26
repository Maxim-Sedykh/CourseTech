using CourseTech.Domain.Dto.UserProfile;
using CourseTech.Domain.Entities;
using MediatR;

namespace CourseTech.Application.CQRS.Commands.UserProfileCommands;

/// <summary>
/// Обновление профиля пользователя самим пользователем.
/// </summary>
/// <param name="UpdateUserProfileDto"></param>
/// <param name="UserProfile"></param>
public record UpdateUserProfileCommand(UpdateUserProfileDto UpdateUserProfileDto, UserProfile UserProfile) : IRequest;
