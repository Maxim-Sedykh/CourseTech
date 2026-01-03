using AutoMapper;
using CourseTech.Application.CQRS.Queries.Dtos.ReviewDtoQueries;
using CourseTech.Domain.Dto.Review;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Extensions;
using CourseTech.Domain.Interfaces.Databases.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CourseTech.Application.CQRS.Handlers.QueryHandlers.DtoQueryHandlers.ReviewDtoQueryHandlers;

public class GetUserReviewDtosHandler(IBaseRepository<Review> reviewRepository, IMapper mapper) : IRequestHandler<GetUserReviewDtosQuery, ReviewDto[]>
{
    public async Task<ReviewDto[]> Handle(GetUserReviewDtosQuery request, CancellationToken cancellationToken)
    {
        return await reviewRepository.GetAll()
                .AsNoTracking()
                .Where(x => x.UserId == request.UserId)
                .Include(x => x.User)
                .AsProjected<Review, ReviewDto>(mapper)
                .ToArrayAsync(cancellationToken);
    }
}
