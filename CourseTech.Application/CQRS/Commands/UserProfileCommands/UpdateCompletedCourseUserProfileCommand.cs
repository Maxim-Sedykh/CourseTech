using CourseTech.Domain.Entities;
using MediatR;

namespace CourseTech.Application.CQRS.Commands.UserProfileCommands;

/// <summary>
/// Обновление профиля пользователя, когда он прошёл курс полностью.
/// </summary>
/// <param name="UserProfile"></param>
/// <param name="Analys"></param>
public record UpdateCompletedCourseUserProfileCommand(UserProfile UserProfile, string Analys) : IRequest;
