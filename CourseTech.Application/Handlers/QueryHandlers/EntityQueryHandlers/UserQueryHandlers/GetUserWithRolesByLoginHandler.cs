﻿using AutoMapper;
using CourseTech.Application.Queries.Entities.UserQueries;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CourseTech.Application.Handlers.QueryHandlers.EntityQueryHandlers.UserQueryHandlers
{
    public class GetUserWithRolesByLoginHandler(IBaseRepository<User> userRepository) : IRequestHandler<GetUserWithRolesByLoginQuery, User>
    {
        public async Task<User> Handle(GetUserWithRolesByLoginQuery request, CancellationToken cancellationToken)
        {
            return await userRepository.GetAll()
                .Include(x => x.Roles)
                .FirstOrDefaultAsync(x => x.Login == request.Login, cancellationToken);
        }
    }
}