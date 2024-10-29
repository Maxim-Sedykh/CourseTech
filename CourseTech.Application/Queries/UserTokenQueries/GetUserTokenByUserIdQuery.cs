﻿using CourseTech.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Application.Queries.UserTokenQueries
{
    public record GetUserTokenByUserIdQuery(Guid UserId) : IRequest<UserToken>;
}
