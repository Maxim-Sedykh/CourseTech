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
    public class GetUpdateUserDtoByUserIdHandler(IBaseRepository<User> userRepository, IMapper mapper) : IRequestHandler<GetUpdateUserDtoByUserIdQuery, UpdateUserDto>
    {
        public async Task<UpdateUserDto> Handle(GetUpdateUserDtoByUserIdQuery request, CancellationToken cancellationToken)
        {
            return await userRepository.GetAll()
                .Include(x => x.UserProfile)
                .Include(x => x.Roles)
                .Where(x => x.Id == request.UserId)
                .Select(x => mapper.Map<UpdateUserDto>(x))
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
