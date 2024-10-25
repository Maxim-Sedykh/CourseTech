using CourseTech.Domain.Dto.Review;
using CourseTech.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Application.Queries.Reviews
{
    public class GetReviewByIdQuery(long reviewId) : IRequest<Review>
    {
        public long ReviewId { get; set; } = reviewId;
    }
}
