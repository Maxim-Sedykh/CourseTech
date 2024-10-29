using AutoMapper;
using CourseTech.Application.Queries.UserQueries;
using CourseTech.DAL.Repositories;
using CourseTech.Domain.Dto.User;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Application.Handlers.UserHandlers
{
    public class GetUserDtosHandler(IBaseRepository<User> userRepository, IMapper mapper) : IRequestHandler<GetUserDtosQuery, UserDto[]>
    {
        public async Task<UserDto[]> Handle(GetUserDtosQuery request, CancellationToken cancellationToken)
        {
            return await userRepository.GetAll()
                    .Include(x => x.Roles)
                    .Select(x => mapper.Map<UserDto>(x))
                    .ToArrayAsync(cancellationToken);
        }
    }
}
