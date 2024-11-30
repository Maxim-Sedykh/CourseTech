using CourseTech.Domain.Dto.User;
using CourseTech.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Application.Queries.Dtos.UserDtoQueries
{
    /// <summary>
    /// Обновить пользователя
    /// </summary>
    /// <param name="UserId"></param>
    public record GetUpdateUserDtoByUserIdQuery(Guid UserId) : IRequest<UpdateUserDto>;
}
