using CourseTech.Application.Queries.LessonQueries;
using CourseTech.Application.Queries.UserQueries;
using CourseTech.Application.Resources;
using CourseTech.Domain.Dto.Lesson;
using CourseTech.Domain.Dto.Lesson.LessonInfo;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Enum;
using CourseTech.Domain.Interfaces.Validators;
using CourseTech.Domain.Result;
using MediatR;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Application.Validations.Validators
{
    public class LessonValidator(ILogger logger) : ILessonValidator
    {
        /// <inheritdoc/>
        public BaseResult ValidateLessonsForUser(UserProfile profile, IEnumerable<LessonDto> lessons)
        {
            if (profile == null)
            {
                return BaseResult<UserLessonsDto>.Failure((int)ErrorCodes.UserProfileNotFound, ErrorMessage.UserProfileNotFound);
            }

            if (!lessons.Any())
            {
                logger.Error(ErrorMessage.LessonsNotFound);

                return BaseResult<UserLessonsDto>.Failure((int)ErrorCodes.LessonsNotFound, ErrorMessage.LessonsNotFound);
            }

            return BaseResult.Success();
        }
    }
}
