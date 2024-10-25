using AutoMapper;
using CourseTech.Application.Queries.Reviews;
using CourseTech.DAL.Repositories;
using CourseTech.Domain.Dto.Review;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Application.Handlers.ReviewHandlers
{
    public class GetUserReviewDtosHandler(IBaseRepository<Review> reviewRepository, IMapper mapper) : IRequestHandler<GetUserReviewDtosQuery, ReviewDto[]>
    {
        public async Task<ReviewDto[]> Handle(GetUserReviewDtosQuery request, CancellationToken cancellationToken)
        {
            return await reviewRepository.GetAll()
                    .AsNoTracking()
                    .Where(x => x.UserId == request.UserId)
                    .Include(x => x.User)
                    .Select(x => mapper.Map<ReviewDto>(x))
                    .ToArrayAsync();
        }
    }
}
