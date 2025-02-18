using AutoMapper;
using CourseTech.Application.CQRS.Queries.Dtos.UserDtoQueries;
using CourseTech.Domain.Dto.User;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Extensions;
using CourseTech.Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CourseTech.Application.CQRS.Handlers.QueryHandlers.DtoQueryHandlers.UserDtoQueryHandlers
{
    public class GetUserDtosHandler(IBaseRepository<User> userRepository, IMapper mapper) : IRequestHandler<GetUserDtosQuery, UserDto[]>
    {
        public async Task<UserDto[]> Handle(GetUserDtosQuery request, CancellationToken cancellationToken)
        {
            return await userRepository.GetAll()
                    .Include(x => x.Roles)
                    .AsProjected<User, UserDto>(mapper)
                    .ToArrayAsync(cancellationToken);
        }
    }
}
