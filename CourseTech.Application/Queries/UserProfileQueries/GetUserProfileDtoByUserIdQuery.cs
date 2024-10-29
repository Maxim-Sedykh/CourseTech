using CourseTech.Domain.Dto.UserProfile;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Application.Queries.UserProfileQueries
{
    public record GetUserProfileDtoByUserIdQuery(Guid UserId) : IRequest<UserProfileDto>;
}
