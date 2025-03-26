using MediatR;

namespace CourseTech.Application.CQRS.Commands.ReviewCommands;

/// <summary>
/// Создание отзыва.
/// </summary>
/// <param name="ReviewText"></param>
/// <param name="UserId"></param>
public record CreateReviewCommand(string ReviewText, Guid UserId) : IRequest;
