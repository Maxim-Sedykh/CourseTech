using CourseTech.Domain.Dto.Review;
using MediatR;

namespace CourseTech.Application.CQRS.Queries.Dtos.ReviewDtoQueries;

/// <summary>
/// Получение отзывов определённого пользователя в виде ReviewDto.
/// </summary>
/// <param name="UserId"></param>
public record GetUserReviewDtosQuery(Guid UserId) : IRequest<ReviewDto[]>;
