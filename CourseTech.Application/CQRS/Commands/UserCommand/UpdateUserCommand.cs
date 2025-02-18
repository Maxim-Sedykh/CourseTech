using CourseTech.Domain.Dto.User;
using CourseTech.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Application.CQRS.Commands.UserCommand
{
    /// <summary>
    /// Обновление информации о пользователе.
    /// </summary>
    /// <param name="UpdateUserDto"></param>
    /// <param name="User"></param>
    public record UpdateUserCommand(UpdateUserDto UpdateUserDto, User User) : IRequest;
}
