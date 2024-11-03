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
    /// <summary>
    /// Получение отзыва по его идентификатору.
    /// </summary>
    /// <param name="ReviewId"></param>
    public record GetReviewByIdQuery(long ReviewId) : IRequest<Review>;
}
