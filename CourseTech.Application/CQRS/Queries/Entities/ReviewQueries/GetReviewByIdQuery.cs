using CourseTech.Domain.Entities;
using MediatR;

namespace CourseTech.Application.CQRS.Queries.Entities.ReviewQueries;

/// <summary>
/// Получение отзыва по его идентификатору.
/// </summary>
/// <param name="ReviewId"></param>
public record GetReviewByIdQuery(long ReviewId) : IRequest<Review>;
