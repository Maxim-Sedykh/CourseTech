﻿using CourseTech.Domain.Dto.Role;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Application.Queries.RoleQueries
{
    public record GetRoleDtosQuery : IRequest<RoleDto[]>;
}