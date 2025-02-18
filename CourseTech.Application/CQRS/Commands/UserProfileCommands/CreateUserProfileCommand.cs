using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Application.CQRS.Commands.UserProfileCommands
{
    /// <summary>
    /// Создание профиля пользователя.
    /// </summary>
    /// <param name="UserId"></param>
    /// <param name="Name"></param>
    /// <param name="Surname"></param>
    /// <param name="DateOfBirth"></param>
    public record CreateUserProfileCommand(Guid UserId, string Name, string Surname, DateTime DateOfBirth) : IRequest;
}
