using CourseTech.Application.Queries.RoleQueries;
using CourseTech.Application.Queries.UserTokenQueries;
using CourseTech.DAL.Repositories;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Application.Handlers.RoleHandlers
{
    public class GetRoleByNameHandler(IBaseRepository<Role> roleRepository) : IRequestHandler<GetRoleByNameQuery, Role>
    {
        public async Task<Role> Handle(GetRoleByNameQuery request, CancellationToken cancellationToken)
        {
            return await roleRepository.GetAll().FirstOrDefaultAsync(x => x.Name == request.Name, cancellationToken);
        }
    }
}
