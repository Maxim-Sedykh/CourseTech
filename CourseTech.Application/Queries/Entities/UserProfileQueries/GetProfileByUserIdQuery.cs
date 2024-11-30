using CourseTech.Domain.Dto.Review;
using CourseTech.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Application.Queries.Entities.UserProfileQueries
{
    /// <summary>
    /// Получение профиля пользователя по идентификатору пользователя.
    /// </summary>
    /// <param name="UserId"></param>
    public record GetProfileByUserIdQuery(Guid UserId) : IRequest<UserProfile>;
}
