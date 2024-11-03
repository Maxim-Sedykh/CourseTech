using CourseTech.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Application.Commands.Reviews
{
    /// <summary>
    /// Удаление отзыва.
    /// </summary>
    /// <param name="Review"></param>
    public record DeleteReviewCommand(Review Review) : IRequest;
}
