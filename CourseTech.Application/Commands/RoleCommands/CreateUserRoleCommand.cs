﻿using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Application.Commands.RoleCommands
{
    public record CreateUserRoleCommand(long RoleId, Guid UserId) : IRequest;
}