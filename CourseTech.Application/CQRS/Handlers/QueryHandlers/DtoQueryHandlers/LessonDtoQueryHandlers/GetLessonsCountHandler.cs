using CourseTech.Application.CQRS.Queries.Dtos.LessonDtoQueries;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CourseTech.Application.CQRS.Handlers.QueryHandlers.DtoQueryHandlers.LessonDtoQueryHandlers
{
    public class GetLessonsCountHandler(IBaseRepository<UserProfile> userProfileRepository) : IRequestHandler<GetLessonsCountQuery, int>
    {
        public async Task<int> Handle(GetLessonsCountQuery request, CancellationToken cancellationToken)
        {
            return await userProfileRepository.GetAll().CountAsync(cancellationToken);
        }
    }
}
