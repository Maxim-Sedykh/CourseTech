﻿using CourseTech.Application.Queries.Entities.RoleQueries;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Application.Handlers.QueryHandlers.EntityQueryHandlers.RoleQueryHandlers
{
    public class GetRoleByIdHandler(IBaseRepository<Role> roleRepository) : IRequestHandler<GetRoleByIdQuery, Role>
    {
        public async Task<Role> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
        {
            return await roleRepository.GetAll().FirstOrDefaultAsync(x => x.Id == request.RoleId, cancellationToken);
        }
    }
}