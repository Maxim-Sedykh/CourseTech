using CourseTech.Domain.Entities;
using MediatR;

namespace CourseTech.Application.CQRS.Commands.ReviewCommands;

/// <summary>
/// Удаление отзыва.
/// </summary>
/// <param name="Review"></param>
public record DeleteReviewCommand(Review Review) : IRequest;
