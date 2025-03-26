using CourseTech.Domain.Entities;
using MediatR;

namespace CourseTech.Application.CQRS.Commands.UserProfileCommands;

/// <summary>
/// Обновление профиля пользователя, когда пользователь создаёт отзыв.
/// </summary>
/// <param name="UserProfile"></param>
public record UpdateProfileReviewsCountCommand(UserProfile UserProfile) : IRequest;
