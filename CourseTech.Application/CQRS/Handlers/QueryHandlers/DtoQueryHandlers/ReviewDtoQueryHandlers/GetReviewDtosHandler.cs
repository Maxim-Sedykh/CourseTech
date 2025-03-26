using AutoMapper;
using CourseTech.Application.CQRS.Queries.Dtos.ReviewDtoQueries;
using CourseTech.Domain.Dto.Review;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Extensions;
using CourseTech.Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CourseTech.Application.CQRS.Handlers.QueryHandlers.DtoQueryHandlers.ReviewDtoQueryHandlers;

public class GetReviewDtosHandler(IBaseRepository<Review> reviewRepository, IMapper mapper) : IRequestHandler<GetReviewDtosQuery, ReviewDto[]>
{
    public async Task<ReviewDto[]> Handle(GetReviewDtosQuery request, CancellationToken cancellationToken)
    {
        return await reviewRepository.GetAll()
                .AsNoTracking()
                .Include(x => x.User)
                .AsProjected<Review, ReviewDto>(mapper)
                .ToArrayAsync(cancellationToken);
    }
}
