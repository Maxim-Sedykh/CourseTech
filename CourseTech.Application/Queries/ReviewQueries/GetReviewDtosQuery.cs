using CourseTech.Domain.Dto.Review;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Application.Queries.Reviews
{
    public class GetReviewDtosQuery : IRequest<ReviewDto[]>;
}
