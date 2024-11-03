using CourseTech.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Application.Commands.UserProfileCommands
{
    /// <summary>
    /// Удаление профиля пользователя.
    /// </summary>
    /// <param name="UserProfile"></param>
    public record DeleteUserProfileCommand(UserProfile UserProfile) : IRequest;
}
