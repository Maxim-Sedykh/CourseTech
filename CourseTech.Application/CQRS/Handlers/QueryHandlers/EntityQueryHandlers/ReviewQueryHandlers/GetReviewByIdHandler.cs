using CourseTech.Application.CQRS.Queries.Entities.ReviewQueries;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CourseTech.Application.CQRS.Handlers.QueryHandlers.EntityQueryHandlers.ReviewQueryHandlers;

public class GetReviewByIdHandler(IBaseRepository<Review> reviewRepository) : IRequestHandler<GetReviewByIdQuery, Review>
{
    public async Task<Review> Handle(GetReviewByIdQuery request, CancellationToken cancellationToken)
    {
        return await reviewRepository.GetAll()
            .FirstOrDefaultAsync(x => x.Id == request.ReviewId, cancellationToken);
    }
}
