using CourseTech.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Application.Commands.UserProfileCommands
{
    public record UpdateCompletedCourseUserProfileCommand(UserProfile UserProfile, string Analys) : IRequest;
}
