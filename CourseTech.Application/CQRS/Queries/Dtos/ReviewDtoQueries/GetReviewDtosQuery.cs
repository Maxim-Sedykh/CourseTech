using CourseTech.Domain.Dto.Review;
using MediatR;

namespace CourseTech.Application.CQRS.Queries.Dtos.ReviewDtoQueries;

/// <summary>
/// Получение всех отзывов в виде ReviewDto
/// </summary>
public record GetReviewDtosQuery : IRequest<ReviewDto[]>;
