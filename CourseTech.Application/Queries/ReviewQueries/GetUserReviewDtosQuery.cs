using CourseTech.Domain.Dto.Review;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Application.Queries.Reviews
{
    /// <summary>
    /// Получение отзывов определённого пользователя в виде ReviewDto.
    /// </summary>
    /// <param name="UserId"></param>
    public record GetUserReviewDtosQuery(Guid UserId) : IRequest<ReviewDto[]>;
}
