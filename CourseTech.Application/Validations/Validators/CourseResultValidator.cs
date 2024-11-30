using CourseTech.Application.Resources;
using CourseTech.Domain.Dto.FinalResult;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Enum;
using CourseTech.Domain.Interfaces.Validators;
using CourseTech.Domain.Result;
using Serilog;

namespace CourseTech.Application.Validations.Validators
{
    public class CourseResultValidator(ILogger logger) : ICourseResultValidator
    {
        /// <inheritdoc/>
        public BaseResult ValidateUserCourseResult(UserProfile profile, int lessonCount)
        {
            if (profile is null)
            {
                return BaseResult<CourseResultDto>.Failure((int)ErrorCodes.UserProfileNotFound, ErrorMessage.UserProfileNotFound);
            }

            if (lessonCount == 0)
            {
                logger.Error(ErrorMessage.LessonsNotFound);

                return BaseResult<CourseResultDto>.Failure((int)ErrorCodes.LessonsNotFound, ErrorMessage.LessonsNotFound);
            }

            return BaseResult.Success();
        }
    }
}
