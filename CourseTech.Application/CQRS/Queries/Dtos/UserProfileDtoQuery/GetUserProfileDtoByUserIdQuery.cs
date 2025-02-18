using CourseTech.Domain.Dto.UserProfile;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Application.CQRS.Queries.Dtos.UserProfileDtoQuery
{
    /// <summary>
    /// Получение UserProfileDto по идентификатору пользователя.
    /// </summary>
    /// <param name="UserId"></param>
    public record GetUserProfileDtoByUserIdQuery(Guid UserId) : IRequest<UserProfileDto>;
}
