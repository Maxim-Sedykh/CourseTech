using CourseTech.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Application.CQRS.Commands.UserProfileCommands
{
    /// <summary>
    /// Обновление профиля пользователя, когда он прошёл урок.
    /// </summary>
    /// <param name="UserProfile"></param>
    /// <param name="UserGrade"></param>
    public record UpdateProfileCompletingLessonCommand(UserProfile UserProfile, float UserGrade) : IRequest;
}
