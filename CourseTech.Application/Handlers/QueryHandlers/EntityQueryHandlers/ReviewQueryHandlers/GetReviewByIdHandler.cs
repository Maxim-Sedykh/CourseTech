using AutoMapper;
using CourseTech.Application.Queries.Entities.ReviewQueries;
using CourseTech.DAL.Repositories;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Application.Handlers.QueryHandlers.EntityQueryHandlers.ReviewQueryHandlers
{
    public class GetReviewByIdHandler(IBaseRepository<Review> reviewRepository) : IRequestHandler<GetReviewByIdQuery, Review>
    {
        public async Task<Review> Handle(GetReviewByIdQuery request, CancellationToken cancellationToken)
        {
            return await reviewRepository.GetAll()
                .FirstOrDefaultAsync(x => x.Id == request.ReviewId, cancellationToken);
        }
    }
}
