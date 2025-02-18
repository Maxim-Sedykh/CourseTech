using CourseTech.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Application.CQRS.Commands.UserProfileCommands
{
    /// <summary>
    /// Обновление профиля пользователя, когда пользователь создаёт отзыв.
    /// </summary>
    /// <param name="UserProfile"></param>
    public record UpdateProfileReviewsCountCommand(UserProfile UserProfile) : IRequest;
}
