﻿using CourseTech.Domain.Dto.FinalResult;
using CourseTech.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Application.Queries.Dtos.UserProfileDtoQuery
{
    /// <summary>
    /// Получить анализ пользователя по его идентификатору.
    /// </summary>
    /// <param name="UserId"></param>
    public record GetAnalysByUserIdQuery(Guid UserId) : IRequest<UserAnalysDto>;
}