using CourseTech.Domain.Dto.UserProfile;
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
    /// Обновление профиля пользователя самим пользователем.
    /// </summary>
    /// <param name="UpdateUserProfileDto"></param>
    /// <param name="UserProfile"></param>
    public record UpdateUserProfileCommand(UpdateUserProfileDto UpdateUserProfileDto, UserProfile UserProfile) : IRequest;
}
