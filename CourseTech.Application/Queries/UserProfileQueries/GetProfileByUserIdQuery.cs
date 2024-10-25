using CourseTech.Domain.Dto.Review;
using CourseTech.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Application.Queries.UserQueries
{
    public class GetProfileByUserIdQuery(Guid userId) : IRequest<UserProfile>
    {
        public Guid UserId { get; set; } = userId;
    }
}
