using CourseTech.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Application.Queries.UserQueries
{
    public record GetUserWithTokenAndRolesByLoginQuery(string Login) : IRequest<User>;
}
