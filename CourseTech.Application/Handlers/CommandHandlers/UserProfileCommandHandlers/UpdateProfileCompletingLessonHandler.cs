using AutoMapper;
using CourseTech.Application.Commands.LessonRecordCommands;
using CourseTech.Application.Commands.UserProfileCommands;
using CourseTech.DAL.Repositories;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Interfaces.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Application.Handlers.CommandHandlers.UserProfileCommandHandlers
{
    public class UpdateProfileCompletingLessonHandler(IBaseRepository<UserProfile> userProfileRepository) : IRequestHandler<UpdateProfileCompletingLessonCommand>
    {
        public Task Handle(UpdateProfileCompletingLessonCommand request, CancellationToken cancellationToken)
        {
            var profile = request.UserProfile;

            profile.CurrentGrade += request.UserGrade;
            profile.LessonsCompleted++;

            userProfileRepository.Update(profile);

            return Task.CompletedTask;
        }
    }
}
