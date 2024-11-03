using CourseTech.Domain.Dto.Review;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Application.Commands.Reviews
{
    /// <summary>
    /// Создание отзыва.
    /// </summary>
    /// <param name="ReviewText"></param>
    /// <param name="UserId"></param>
    public record CreateReviewCommand(string ReviewText, Guid UserId): IRequest;
}
