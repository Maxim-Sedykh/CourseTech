using CourseTech.Application.Queries.LessonQueries;
using CourseTech.Application.Queries.UserQueries;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Application.Handlers.LessonHandlers
{
    public class GetLessonsCountHandler(IBaseRepository<UserProfile> userProfileRepository) : IRequestHandler<GetLessonsCountQuery, int>
    {
        public async Task<int> Handle(GetLessonsCountQuery request, CancellationToken cancellationToken)
        {
            return await userProfileRepository.GetAll().CountAsync(cancellationToken);
        }
    }
}
